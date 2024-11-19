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
            TransactionScope transactionScope;
            try
            {
                var productNameLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                using (transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
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


                        if (productRepository.IsEmptyId(product.Id) ){                           
                            id = await productRepository.Create(productToSaveOrCreate);
                        }
                        else
                        {
                           var existingProduct = productRepository.GetById(product.Id);
                            await productRepository.Update(productToSaveOrCreate);
                        }

                        productNameLookup.Add(productToSaveOrCreate.Name, productToSaveOrCreate.Id);
                    }

                    transactionScope.Complete();
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

    }
}
