﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proba.Server;

namespace Proba.Server.Migrations
{
    [DbContext(typeof(ProbaContext))]
    [Migration("20190724023640_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Greet.ProbaMessage", b =>
                {
                    b.Property<string>("Greeting")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<double>("Value");

                    b.HasKey("Greeting");

                    b.ToTable("Probas");
                });
#pragma warning restore 612, 618
        }
    }
}
