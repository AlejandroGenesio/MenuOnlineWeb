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

        public async Task Import (ProductBulkImportDTO importContainer)
        {
            // Loop over each product
            try
            {
                var productNameLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    // Save products
                    foreach(var product in importContainer.Products)
                    {
                        var productEntityToSave = mapper.Map<Product>(product);
                        // TODO: check for null
                       
                        // Buscar por ID
                        // Por nombre...

                        int id = await productRepository.Create(productEntityToSave);


                        productNameLookup.Add(productEntityToSave.Name, id);
                    }

                    // Save Variants

                    foreach (var variant in importContainer.Variants)
                    {
                        if (!productNameLookup.TryGetValue(variant.ProductName??string.Empty, out int productId))
                        {
                            // TODO: Error, 
                            continue;

                        }
                      

                        var variantToSave = new CreateVariantDTO
                        {
                            Name = variant.Name,
                            Description = variant.Description,
                            price = variant.price,
                             stock = variant.stock                             
                        };


                        var variantEntity = mapper.Map<Variant>(variantToSave);
                        variantEntity.ProductId = productId;

                        var id = await repositoryVariants.Create(variantEntity);
                    }

                }
            }
            catch (Exception e)
            {
                productRepository.DiscardChanges();
                throw;
            }

        

            // Map produt name to product Id
            // Rollback if fail
            
            // Create product variant dto  with product ID

            return;
        }

    }
}
