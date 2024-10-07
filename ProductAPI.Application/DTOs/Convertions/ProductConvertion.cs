using ProductAPI.Domain.Entities;


namespace ProductAPI.Application.DTOs.Convertions
{
    public static class ProductConvertion
    {
        public static Product ToEntity(ProductDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Quantity = dto.Quantity,
            Price = dto.Price
        };

        public static (ProductDto?, IEnumerable<ProductDto>?) FomEntity(Product product, IEnumerable<Product>? products)
        {
            // return singal 
            if (product is not null || products is null)
            {
                var singalProd = new ProductDto
                (
                     product!.Id,
                     product!.Name,
                     product!.Quantity,
                    product!.Price
                );
                return (singalProd, null);
            }

            // return list 

            // return singal 
            if (products is not null || product is null)
            {
                var _products = products.Select(p =>
                new ProductDto(
                    product!.Id,
                     product!.Name,
                     product!.Quantity,
                    product!.Price)
                ).ToList();
                return (null, _products);
            }

            return (null, null);
        }
    }
}
