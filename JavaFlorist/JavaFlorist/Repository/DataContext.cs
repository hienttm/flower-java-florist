using System;
using JavaFlorist.Models;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repository
{
	public class DataContext:DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<BannerContentModel> BannerContents { get; set; }
        public DbSet<BannerModel> Banners { get; set; }
        public DbSet<DiscountModel> Discounts { get; set; }
        public DbSet<MediaModel> Medias { get; set; }
        public DbSet<OccasionModel> Occasions { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderStatusModel> OrderStatuses { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public DbSet<RateModel> Rates { get; set; }
    }
}

