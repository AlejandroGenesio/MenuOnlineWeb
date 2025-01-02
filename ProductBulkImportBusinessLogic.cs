using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using System.Linq;
using System.Transactions;

namespace MenuOnlineUdemy
{
    public class ProductBulkImportBusinessLogic
    {
        private readonly IMapper mapper;
        private readonly IRepositoryProducts productRepository;
        private readonly IRepositoryVariants variantsRepository;
        private readonly IRepositoryModifierGroups modifierGroupsRepository;
        private readonly IRepositoryModifierOptions modifierOptionsRepository;

        public ProductBulkImportBusinessLogic(IMapper mapper,
            IRepositoryProducts productRepository,
            IRepositoryVariants repositoryVariants,
            IRepositoryModifierGroups modifierGroupsRepository,
            IRepositoryModifierOptions modifierOptionsRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.variantsRepository = repositoryVariants;
            this.modifierGroupsRepository = modifierGroupsRepository;
            this.modifierOptionsRepository = modifierOptionsRepository;
        }

        public async Task Import(ProductBulkImportDTO importContainer)
        {
            // Loop over each product
            try
            {

                var productNameLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                var productVariantsGrouped = importContainer.Variants.GroupBy(v => v.ProductName, StringComparer.OrdinalIgnoreCase);

                if (productVariantsGrouped.Any(k => string.IsNullOrWhiteSpace(k.Key)))
                {
                    //TODO: Add error. Product name can't be null in Variants
                    return;
                }
                var productVariants = productVariantsGrouped.ToDictionary(k => k.Key, StringComparer.OrdinalIgnoreCase);


                // Save products
                foreach (ProductDTO product in importContainer.Products)
                {
                    int id;

                    var createProductDto = new CreateProductDTO
                    {
                        Description = product.Description,
                        Name = product.Name,
                        Price = product.Price,
                        SellMinOptions = product.SellMinOptions,
                        SellMinPrice = product.SellMinPrice
                    };

                    var productToSaveOrCreate = mapper.Map<Product>(createProductDto);


                    if (productRepository.IsEmptyId(product.Id))
                    {
                        id = await productRepository.Create(productToSaveOrCreate);
                    }
                    else
                    {
                        var existingProduct = productRepository.GetById(product.Id);
                        await productRepository.Update(productToSaveOrCreate);
                    }

                    // Handle variants
                    var currentProductVariants = productVariants.GetValueOrDefault(productToSaveOrCreate.Name) ?? Enumerable.Empty<ImportProductVariantDTO>();
                    await HandleVariants(productToSaveOrCreate, currentProductVariants);

                }

                await ImportModifierGroups(importContainer.ModifierGroups);

                await AssignProductModifierGroups(importContainer);

                await AssignModifierGroupOptions(importContainer);



                async Task HandleVariants(Product product, IEnumerable<ImportProductVariantDTO> variantsToCreate)
                {
                    foreach (var productVariant in variantsToCreate)
                    {
                        if (variantsRepository.IsEmptyId(productVariant.Id))
                        {

                            var variantToCreate = new CreateVariantDTO
                            {
                                Description = productVariant.Description,
                                Name = productVariant.Name,
                                price = productVariant.Price.GetValueOrDefault(),
                                stock = productVariant.Stock
                            };

                            await CreateProductVariant(product, variantToCreate);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                //productRepository.DiscardChanges();                
                throw;
            }



            // Map produt name to product Id
            // Rollback if fail

            // Create product variant dto  with product ID

            return;
        }

        private async Task AssignModifierGroupOptions(ProductBulkImportDTO importContainer)
        {
            var optionsByGroup = importContainer.ModifierGroupOptions.GroupBy(option => option.GroupOptionName);
            foreach (var modifierOptionsByGroup in optionsByGroup)
            {
                List<ModifierGroup> groupOptions = await modifierGroupsRepository.GetByGroupOptionsName(modifierOptionsByGroup.Key);
                // Assign Options to each group

                foreach (ModifierGroup group in groupOptions)
                {
                    // Clear all options, to assign them later
                    group.ModifierGroupOptions.Clear();
                    await modifierGroupsRepository.Update(group);

                    foreach (var modifierOption in modifierOptionsByGroup)
                    {
                        ModifierOption optionToMap;

                        if (await modifierOptionsRepository.IfExists(modifierOption.Id))
                        {
                            // Exists, update
                            optionToMap = await modifierOptionsRepository.GetById(modifierOption.Id);

                            if (optionToMap == null)
                                continue;

                            optionToMap.Description = modifierOption.Description;
                            optionToMap.Name = modifierOption.Name;
                            optionToMap.Price = optionToMap.Price;

                            await modifierOptionsRepository.Update(optionToMap);
                        }
                        else
                        {
                            // Create new
                            optionToMap = new()
                            {
                                Id = modifierOption.Id,
                                Description = modifierOption.Description,
                                Name = modifierOption.Name,
                                Price = modifierOption.Price.GetValueOrDefault(),
                                ModifierGroup = group
                            };

                            await modifierOptionsRepository.Create(optionToMap);
                        }
                    }

                }
            }
        }

        private async Task AssignProductModifierGroups(ProductBulkImportDTO importDto)
        {
            foreach (var productName in importDto.ProductNamesForProductModifierGroupMapping)
            {
                // TODO: name is a unique key


                //var product = await productRepository.FindByName(productName);

                // TODO: clean database. Add unique constraint for Products
                var product = (await productRepository.GetByName(productName)).FirstOrDefault();
                if (product == null)
                {
                    continue;
                }

                List<int> modifierGroupsToAssign = importDto.ModifierGroups
                    .Where(m => m.MappedWithProductNames.Contains(product.Name!))
                    .Select(
                    m => m.Id).ToList();

                await productRepository.AssignModifierGroup(product.Id, modifierGroupsToAssign);
            }
        }

        private async Task ImportModifierGroups(IReadOnlyCollection<ImportModifierGroupDTO> modifierGroups)
        {
            foreach (var modifierGroupDTO in modifierGroups)
            {
                var createDto = new CreateModifierGroupDTO
                {
                    ExtraPrice = modifierGroupDTO.ExtraPrice,
                    GroupStyle = modifierGroupDTO.GroupStyle,
                    GroupStyleClosed = modifierGroupDTO.GroupStyleClosed,
                    OptionsGroup = modifierGroupDTO.OptionsGroup,
                    Label = modifierGroupDTO.Label
                };

                var modifierGroup = mapper.Map<ModifierGroup>(createDto);

                var exists = await modifierGroupsRepository.IfExists(modifierGroupDTO.Id);
                if (!exists)
                {
                    var createdEntityId = await modifierGroupsRepository.Create(modifierGroup);
                    modifierGroupDTO.Id = createdEntityId;
                }
                else
                {
                    modifierGroup.Id = modifierGroupDTO.Id;
                    await modifierGroupsRepository.Update(modifierGroup);
                }
            }
        }

        public async Task CreateProductVariant(Product product, CreateVariantDTO createVariantDTO)
        {
            var variant = mapper.Map<Variant>(createVariantDTO);
            variant.ProductId = product.Id;

            var id = await variantsRepository.Create(variant);

            //product.Variants.Add(variant);

            // TODO:  create method for this
            //await productRepository.Update(product);

        }


    }
}
