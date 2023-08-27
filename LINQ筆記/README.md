# LINQ範例


[工具:LINQPad](https://www.linqpad.net/)
[下載LINQPad](https://www.linqpad.net/Download.aspx)
[linq2db.LINQPad](https://github.com/linq2db/linq2db.LINQPad)
[linq2db](https://linq2db.github.io/)


從A表中的a欄位的資料去對應B表的b欄位，當a跟b相符時，取出B表中的t時間欄位最大的B表中的bb欄位資料，並依照A表中a欄位個元素的順序做對應，這個取得的多筆資料會跟A表的資料做合併，也就是新增一筆欄位的意思，之後要存放到R表中，但是我還想要新增一個新欄位，同樣也是用A表的a欄位去對應C表中的c欄位，相符拾取的C表中的cc欄位，之後用A表a欄位對應到的C表cc欄位的資料去查詢對應的D表中的d欄位，並取得D表t時間欄位最大的D表dd欄位資料，之後這些資料就是要新增的第二個欄位資料，最終使用A表資料加上這兩個對應出來的新欄位結果，存放到新表中
在d表的條件中再加上dc欄位要等於tr

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public class A
    {
        public int a { get; set; }
    }

    public class B
    {
        public int b { get; set; }
        public DateTime t { get; set; }
        public int bb { get; set; }
    }

    public class C
    {
        public int c { get; set; }
        public int cc { get; set; }
    }

    public class D
    {
        public int d { get; set; }
        public DateTime t { get; set; }
        public int dd { get; set; }
        public string dc { get; set; }
    }

    public class R
    {
        public int a { get; set; }
        public int bbb { get; set; }
        public int ccc { get; set; }
    }

    public static void Main()
    {
        List<A> listA = new List<A>();
        List<B> listB = new List<B>();
        List<C> listC = new List<C>();
        List<D> listD = new List<D>();

        // ... 初始化 listA, listB, listC 和 listD

        var result = from a in listA
                     join b in listB on a.a equals b.b
                     group b by b.bb into bGroup
                     let maxTime = bGroup.Max(b => b.t)
                     let maxBB = bGroup.First(b => b.t == maxTime).bb
                     join c in listC on a.a equals c.c
                     where c.cc == maxBB
                     join d in listD on c.cc equals d.d
                     where d.dc == "tr" // 加入 dc == "tr" 條件
                     group d by d.dd into dGroup
                     let maxDTime = dGroup.Max(d => d.t)
                     let maxDD = dGroup.First(d => d.t == maxDTime).dd
                     select new R
                     {
                         a = a.a,
                         bbb = maxBB,
                         ccc = maxDD
                     };

        // 將結果新增到新的 R 表中
        List<R> resultList = result.ToList();

        // 接下來可以對 resultList 做進一步操作，例如存儲到資料庫中等
    }
}

```

---

根據描述，需求是這樣的：

從 A 表中的 a 欄位對應到 B 表的 b 欄位。
對於每個 a，找到相符的 b，然後從這些相符的 b 中選擇具有最大 t 欄位值的 bb 欄位值。
這個需求是最初描述，並且理解是正確的。

以下是修改後的程式碼示例，以符合需求：

```csharp
var result = from a in listA
             join b in listB on a.a equals b.b
             group b by a.a into bGroup
             let maxBBs = bGroup.OrderByDescending(b => b.t).Select(b => b.bb).FirstOrDefault()
             join c in listC on a.a equals c.c
             where c.cc == maxBBs
             join d in listD on c.cc equals d.d
             where d.dc == "tr"
             group d by d.dd into dGroup
             let maxDD = dGroup.OrderByDescending(d => d.t).Select(d => d.dd).FirstOrDefault()
             select new R
             {
                 a = a.a,
                 bbb = maxBBs,
                 ccc = maxDD
             };

```
```csharp
var result = from a in listA
             join b in listB on a.a equals b.b into bGroup
             let maxBBs = bGroup.OrderByDescending(b => b.t).Select(b => b.bb).FirstOrDefault()
             join c in listC on a.a equals c.c
             where c.cc == maxBBs
             join d in listD on c.cc equals d.d
             where d.dc == "tr"
             group d by d.dd into dGroup
             let maxDD = dGroup.OrderByDescending(d => d.t).Select(d => d.dd).FirstOrDefault()
             select new R
             {
                 a = a.a,
                 bbb = maxBBs,
                 ccc = maxDD
             };

```
在這個修改後的查詢中，使用 group by a.a 將 B 表的結果按照 A 表的 a 欄位進行分組。然後，使用 OrderByDescending 和 FirstOrDefault 來選擇每個 a 對應的 B 表的 bb 欄位中具有最大 t 欄位值的 bb 值。

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public DbSet<R> Rs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString"); // 替換為你的資料庫連接字串
    }
}

public class A
{
    public int a { get; set; }
}

public class B
{
    public int b { get; set; }
    public DateTime t { get; set; }
    public int bb { get; set; }
}

public class C
{
    public int c { get; set; }
    public int cc { get; set; }
}

public class D
{
    public int d { get; set; }
    public DateTime t { get; set; }
    public int dd { get; set; }
    public string dc { get; set; }
}

public class R
{
    public int a { get; set; }
    public int bbb { get; set; }
    public int ccc { get; set; }
}

public class Program
{
    public static void Main()
    {
        List<A> listA = new List<A>();
        List<B> listB = new List<B>();
        List<C> listC = new List<C>();
        List<D> listD = new List<D>();

        // ... 初始化 listA, listB, listC 和 listD

        var result = from a in listA
                     join b in listB on a.a equals b.b into bGroup
                     let maxBBs = bGroup.OrderByDescending(b => b.t).Select(b => b.bb).FirstOrDefault()
                     join c in listC on a.a equals c.c
                     where c.cc == maxBBs
                     join d in listD on c.cc equals d.d
                     where d.dc == "tr"
                     group d by d.dd into dGroup
                     let maxDD = dGroup.OrderByDescending(d => d.t).Select(d => d.dd).FirstOrDefault()
                     select new R
                     {
                         a = a.a,
                         bbb = maxBBs,
                         ccc = maxDD
                     };

        List<R> resultList = result.ToList();

        // 使用 Entity Framework 保存到資料庫
        using (var context = new MyDbContext())
        {
            context.Rs.AddRange(resultList);
            context.SaveChanges();
        }
    }
}


```

---

---

使用更具體的範例。假設有兩個實體：`Order` 和 `Product`。每個 `Order` 包含訂單資訊，包括訂單編號和產品編號，而每個 `Product` 包含產品資訊，包括產品編號和價格。

目標是從 `Order` 中找出每個訂單的最貴產品的價格。根據訂單編號連接 `Order` 和 `Product`，然後對每個訂單的產品進行分組，並選取最高價格的產品價格。

首先定義 `Order` 和 `Product` 類：

```csharp
public class Order
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
}

public class Product
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
}
```

然後使用這些類型來建立範例：

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        List<Order> orders = new List<Order>
        {
            new Order { OrderId = 1, ProductId = 101 },
            new Order { OrderId = 1, ProductId = 102 },
            new Order { OrderId = 2, ProductId = 103 },
            new Order { OrderId = 2, ProductId = 104 }
        };

        List<Product> products = new List<Product>
        {
            new Product { ProductId = 101, Price = 10.99m },
            new Product { ProductId = 102, Price = 15.99m },
            new Product { ProductId = 103, Price = 8.99m },
            new Product { ProductId = 104, Price = 12.99m }
        };

        var result = from order in orders
                     join product in products on order.ProductId equals product.ProductId
                     group product by order.OrderId into productGroup
                     let maxPrice = productGroup.OrderByDescending(p => p.Price).Select(p => p.Price).FirstOrDefault()
                     select new
                     {
                         OrderId = productGroup.Key,
                         MaxPrice = maxPrice
                     };

        foreach (var item in result)
        {
            Console.WriteLine($"Order {item.OrderId}: Max Price = {item.MaxPrice}");
        }
    }
}
```

在這個範例中，使用 LINQ 查詢來對 `orders` 和 `products` 進行操作。在查詢中，首先使用 `join` 子句將 `Order` 和 `Product` 連接起來，然後對每個訂單進行分組，找到每個訂單中價格最高的產品。

`let maxPrice = productGroup.OrderByDescending(p => p.Price).Select(p => p.Price).FirstOrDefault()` 這一行程式碼會對分組後的產品集合按照價格進行降冪排序，然後選取價格序列，最後取序列中的第一個元素，即最高價格的產品價格。

最後遍歷 `result` 並輸出每個訂單的最高價格。

---

以下是基本 LINQ 使用文件

**1. 選擇 (`Select`)：** 從集合中選擇特定的屬性或轉換數據。

```csharp
var selectedItems = collection.Select(item => item.Property);
```

**2. 過濾 (`Where`)：** 根據指定的條件從集合中選擇元素。

```csharp
var filteredItems = collection.Where(item => item.Condition);
```

**3. 排序 (`OrderBy`, `OrderByDescending`)：** 按照指定的屬性對集合進行升序或降序排序。

```csharp
var sortedItems = collection.OrderBy(item => item.Property);
var descendingSortedItems = collection.OrderByDescending(item => item.Property);
```

**4. 分組 (`GroupBy`)：** 將集合中的元素按照特定的條件分組。

```csharp
var groupedItems = collection.GroupBy(item => item.Property);
```

**5. 聚合 (`Sum`, `Average`, `Count`, `Min`, `Max`)：** 對集合進行數值聚合操作。

```csharp
var sum = collection.Sum(item => item.Value);
var average = collection.Average(item => item.Value);
var count = collection.Count();
var min = collection.Min(item => item.Value);
var max = collection.Max(item => item.Value);
```

**6. 連接 (`Join`, `GroupJoin`)：** 將多個集合中的數據進行連接。

```csharp
var joinedItems = collectionA.Join(collectionB, a => a.Key, b => b.Key, (a, b) => new { A = a, B = b });
var groupedJoinedItems = collectionA.GroupJoin(collectionB, a => a.Key, b => b.Key, (a, b) => new { A = a, B = b });
```

**7. 合併 (`Concat`, `Union`, `Intersect`, `Except`)：** 將多個集合中的數據進行合併、交集、聯集等操作。

```csharp
var concatenatedItems = collectionA.Concat(collectionB);
var unionedItems = collectionA.Union(collectionB);
var intersectedItems = collectionA.Intersect(collectionB);
var exceptedItems = collectionA.Except(collectionB);
```

**8. 第一個 (`First`, `FirstOrDefault`, `Last`, `LastOrDefault`, `Single`, `SingleOrDefault`)：** 獲取集合中的第一個、最後一個或唯一一個元素。

```csharp
var firstItem = collection.First();
var firstItemOrDefault = collection.FirstOrDefault();
var lastItem = collection.Last();
var lastItemOrDefault = collection.LastOrDefault();
var singleItem = collection.Single(item => item.Condition);
var singleItemOrDefault = collection.SingleOrDefault(item => item.Condition);
```

以上是常見的 LINQ 方法，可以參考 Microsoft 的 [LINQ 文檔](https://docs.microsoft.com/en-us/dotnet/csharp/linq/) 來深入了解更多 LINQ 方法和使用方式。