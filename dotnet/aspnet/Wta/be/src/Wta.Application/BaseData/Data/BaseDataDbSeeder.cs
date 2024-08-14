using Wta.Application.BaseData.Domain;
using Wta.Application.System.Data;

namespace Wta.Application.BaseData.Data;

public class BaseDataDbSeeder : IDbSeeder<DefaultDbContext>
{
    public void Seed(DefaultDbContext context)
    {
        //产品类型
        context.Set<ProductType>().AddRange([new ProductType()
        {
            Id = context.NewGuid(),
            Name = "主料",
            Number = "01"
        }.UpdateNode(),new ProductType(){
            Id = context.NewGuid(),
            Name = "主料",
            Number = "02"
        }.UpdateNode()]);

        //产品分类
        context.Set<ProductCategory>().AddRange([new ProductCategory()
        {
            Id = context.NewGuid(),
            Name = "CPU芯片",
            Number = "01"
        }.UpdateNode(),new ProductCategory(){
            Id = context.NewGuid(),
            Name = "散热风扇",
            Number = "02"
        }.UpdateNode()]);

        //仓库类型
        context.Set<WarehouseType>().AddRange([new WarehouseType()
        {
            Id = context.NewGuid(),
            Name = "存货仓",
            Number = "01"
        }.UpdateNode(),new WarehouseType(){
            Id = context.NewGuid(),
            Name = "现场仓",
            Number = "02"
        }.UpdateNode(),new WarehouseType(){
            Id = context.NewGuid(),
            Name = "废品仓",
            Number = "03"
        }.UpdateNode()]);

        //仓库
        context.Set<Warehouse>().AddRange([new Warehouse()
        {
            Id = context.NewGuid(),
            Name = "杭州仓",
            Number = "01"
        }.UpdateNode(),new Warehouse(){
            Id = context.NewGuid(),
            Name = "无锡仓",
            Number = "02"
        }.UpdateNode()]);

        //库位

        //仓库
        context.Set<Warehouse>().AddRange([new Warehouse()
        {
            Id = context.NewGuid(),
            Name = "杭州仓01",
            Number = "01"
        }.UpdateNode(),new Warehouse(){
            Id = context.NewGuid(),
            Name = "无锡仓02",
            Number = "02"
        }.UpdateNode()]);

        //保存
        context.SaveChanges();
    }
}
