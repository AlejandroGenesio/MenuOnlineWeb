using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using System.Transactions;

namespace MenuOnlineUdemy
{
    public class ProductBulkImportBusinessLogic
    {
        private readonly IMapper mapper;
        private readonly IRepositoryProducts productRepository;
        private readonly IRepositoryVariants repositoryVariants;
        public ProductBulkImportBusinessLogic(IMapper mapper, IRepositoryProducts productRepository, IRepositoryVariants repositoryVariants)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.repositoryVariants = repositoryVariants;
        }

        public async Task Import(ProductBulkImportDTO importContainer)
        {
            // Loop over each product
            try
            {
                var productNameLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                
                var productVariantsGrouped = importContainer.Variants.GroupBy(v => v.ProductName, StringComparer.OrdinalIgnoreCase);
                var productVariants = productVariantsGrouped.ToDictionary(k=>k.Key, StringComparer.OrdinalIgnoreCase);

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
                    var currentProductVariants = productVariants.GetValueOrDefault(productToSaveOrCreate.Name);
                    await HandleVariants(productToSaveOrCreate.Id, currentProductVariants);
                }


                async Task HandleVariants(int productId, IEnumerable<ImportProductVariantDTO> variantsToCreate)
                {
                    foreach(var productVariant in variantsToCreate)
                    {
                        if (!productVariant.Id.HasValue)
                        {

                            var variantToCreate = new CreateVariantDTO
                            {
                                Description = productVariant.Description,
                                Name = productVariant.Name,
                                price = productVariant.Price,
                                stock = productVariant.Stock
                            };

                            await CreateProductVariant(productId,variantToCreate);
                        }
                    }
                }

                // Save Variants

                //foreach (var variant in importContainer.Variants)
                //{
                //    if (!productNameLookup.TryGetValue(variant.ProductName??string.Empty, out int productId))
                //    {
                //        // TODO: Error, 
                //        continue;

                //    }


                //    var variantToSave = new CreateVariantDTO
                //    {
                //        Name = variant.Name,
                //        Description = variant.Description,
                //        price = variant.Price,
                //         stock = variant.Stock                             
                //    };


                //    var variantEntity = mapper.Map<Variant>(variantToSave);
                //    variantEntity.ProductId = productId;

                //    var id = await repositoryVariants.Create(variantEntity);
                //}

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

        public async Task CreateProductVariant(int productId, CreateVariantDTO createVariantDTO)
        {           
            var variant = mapper.Map<Variant>(createVariantDTO);
            variant.ProductId = productId;

            var id = await repositoryVariants.Create(variant);

        }

    }
}
