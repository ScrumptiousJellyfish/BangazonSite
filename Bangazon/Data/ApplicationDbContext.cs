﻿using System;
using System.Collections.Generic;
using System.Text;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            base.OnModelCreating (modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Order> ()
                .Property (b => b.DateCreated)
                .HasDefaultValueSql ("GETDATE()");

            // Restrict deletion of related order when OrderProducts entry is removed
            modelBuilder.Entity<Order> ()
                .HasMany (o => o.OrderProducts)
                .WithOne (l => l.Order)
                .OnDelete (DeleteBehavior.Restrict);

            modelBuilder.Entity<Product> ()
                .Property (b => b.DateCreated)
                .HasDefaultValueSql ("GETDATE()");

            // Restrict deletion of related product when OrderProducts entry is removed
            modelBuilder.Entity<Product> ()
                .HasMany (o => o.OrderProducts)
                .WithOne (l => l.Product)
                .OnDelete (DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentType> ()
                .Property (b => b.DateCreated)
                .HasDefaultValueSql ("GETDATE()");


            ApplicationUser user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                StreetAddress = "123 Infinity Way",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            ApplicationUser user2 = new ApplicationUser
            {
                FirstName = "Jenn",
                LastName = "TheDestroyer",
                StreetAddress = "1516 Elm Run Ct",
                UserName = "jenn@jennhatesme.com",
                NormalizedUserName = "JENN@JENNHATESME.COM",
                Email = "elane@seinfeld.net",
                NormalizedEmail = "ELANE@SEINFELD.NET",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            var passwordHash2 = new PasswordHasher<ApplicationUser>();
            user2.PasswordHash = passwordHash2.HashPassword(user2, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user2);


            ProductType ProductType1 = new ProductType()
            {
                ProductTypeId = 1,
                Label = "Food"
            };

            ProductType ProductType2 = new ProductType()
            {
                ProductTypeId = 2,
                Label = "Electronics"
            };

            ProductType ProductType3 = new ProductType()
            {
                ProductTypeId = 3,
                Label = "Clothing"
            };

            ProductType ProductType4 = new ProductType()
            {
                ProductTypeId = 4,
                Label = "Homewares"
            };

            modelBuilder.Entity<ApplicationUser>().HasData(ProductType1);
            modelBuilder.Entity<ApplicationUser>().HasData(ProductType2);
            modelBuilder.Entity<ApplicationUser>().HasData(ProductType3);
            modelBuilder.Entity<ApplicationUser>().HasData(ProductType4);


            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType()
                {
                    PaymentTypeId = 1,
                    UserId = user.Id,
                    Description = "American Express",
                    AccountNumber = "86753095551212"
                },
                new PaymentType()
                {
                    PaymentTypeId = 2,
                    UserId = user.Id,
                    Description = "Discover",
                    AccountNumber = "4102948572991"
                },
                new PaymentType()
                {
                    PaymentTypeId = 3,
                    UserId = user2.Id,
                    Description = "Discover",
                    AccountNumber = "9992948572991"
                }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order()
                {
                    OrderId = 1,
                    DateCreated = new DateTime(2018, 9, 01),
                    DateCompleted = new DateTime(2018, 10, 01),
                    User = user,
                    PaymentTypeId = 1,
                },
                new Order()
                {
                    OrderId = 2,
                    DateCreated = new DateTime(2017, 3, 01),
                    DateCompleted = new DateTime(2018, 10, 01),
                    User = user2,
                    PaymentTypeId = 3,
                },
                new Order()
                {
                    OrderId = 3,
                    DateCreated = new DateTime(2016, 9, 01),
                    DateCompleted = new DateTime(2017, 10, 01),
                    User = user,
                    PaymentTypeId = 2,
                },
                new Order()
                {
                    OrderId = 4,
                    DateCreated = new DateTime(2018, 9, 01),
                    DateCompleted = null,
                    User = user2,
                    PaymentTypeId = null,
                },
                new Order()
                {
                    OrderId = 5,
                    DateCreated = new DateTime(2018, 10, 2),
                    DateCompleted = null,
                    User = user,
                    PaymentTypeId = null,
                }


            );

            modelBuilder.Entity<OrderProduct>().HasData(
                new OrderProduct()
                {
                    OrderProductId = 1,
                    OrderId = 1,
                    ProductId = 1
                },
                new OrderProduct()
                {
                    OrderProductId = 2,
                    OrderId = 1,
                    ProductId = 7
                },
                new OrderProduct()
                {
                    OrderProductId = 3,
                    OrderId = 2,
                    ProductId = 6
                },
                new OrderProduct()
                {
                    OrderProductId = 4,
                    OrderId = 2,
                    ProductId = 4
                },
                new OrderProduct()
                {
                    OrderProductId = 5,
                    OrderId = 3,
                    ProductId = 8
                },
                new OrderProduct()
                {
                    OrderProductId = 6,
                    OrderId = 4,
                    ProductId = 2
                },
                new OrderProduct()
                {
                    OrderProductId = 7,
                    OrderId = 5,
                    ProductId = 3
                }
                );
            Product product1 = new Product()
            {
                ProductId = 1,
                Description = "Banana Daniels",
                Title = "Bananiels",
                Price = 17.01,
                User = user,
                ProductTypeId = 1,
                Quantity = 3
            };

            Product product2 = new Product()
            {
                ProductId = 2,
                Description = "It dries the hairs or else it gets the hose again",
                Title = "Hair-O-Matic 9000",
                Price = 25.00,
                User = user,
                ProductTypeId = 2,
                Quantity = 123
            };

            Product product3 = new Product()
            {
                ProductId = 3,
                Description = "Provides +1 to poppable collars",
                Title = "Ralph Lauren Polo",
                Price = 30.00,
                User = user,
                ProductTypeId = 3,
                Quantity = 754
            };

            Product product4 = new Product()
            {
                ProductId = 4,
                Description = "Plug in to the Adventure!",
                Title = "Brave Little Toaster",
                Price = 10.00,
                User = user,
                ProductTypeId = 4,
                Quantity = 5
            };

            Product product5 = new Product()
            {
                ProductId = 5,
                Description = "That pizza with the cheese in the crust",
                Title = "Stuffed Crust Digiorno",
                Price = 14.00,
                User = user,
                ProductTypeId = 1,
                Quantity = 34
            };

            Product product6 = new Product()
            {
                ProductId = 6,
                Description = "Cool.",
                Title = "Automated Fidget Spinner",
                Price = 6000.00,
                User = user,
                ProductTypeId = 2,
                Quantity = 87
            };

            Product product7 = new Product()
            {
                ProductId = 7,
                Description = "No Stripes or Polka Dots",
                Title = "Heather Gray Hoodie",
                Price = 70.00,
                User = user,
                ProductTypeId = 3,
                Quantity = 7
            };

            Product product8 = new Product()
            {
                ProductId = 8,
                Description = "Tear down the establishment of mars.",
                Title = "Sledgehammer",
                Price = 830.00,
                User = user,
                ProductTypeId = 4,
                Quantity = 10
            };

            modelBuilder.Entity<Product>().HasData(product1);
            modelBuilder.Entity<Product>().HasData(product2);
            modelBuilder.Entity<Product>().HasData(product3);
            modelBuilder.Entity<Product>().HasData(product4);
            modelBuilder.Entity<Product>().HasData(product5);
            modelBuilder.Entity<Product>().HasData(product6);
            modelBuilder.Entity<Product>().HasData(product7);
            modelBuilder.Entity<Product>().HasData(product8);


        }
    }
}