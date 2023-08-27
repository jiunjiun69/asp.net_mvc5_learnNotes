# ASP.NET MVC - Code First - Model層設計(一對一、一對多、多對多)

[https://dotblogs.com.tw/kevin_blog/2018/01/31/143252](https://dotblogs.com.tw/kevin_blog/2018/01/31/143252 "smartCard-inline")

‌

[https://learn.microsoft.com/zh-tw/ef/core/modeling/relationships/one-to-many](https://learn.microsoft.com/zh-tw/ef/core/modeling/relationships/one-to-many "smartCard-inline")

‌

# **一對一:**

兩Model之間，各自包含對方的導覽屬性，就被當成一對一的關係

```csharp
public class TableA
{
   public int TableAId { get; set; }

   public string name { get; set; }

   public virtual TableB tableB { get; set; }
}


public class TableB
{
   public int TableBId { get; set; }

   public string name { get; set; }

   public TableA tablea { get; set; }
}
```

必須在DbContext當中使用Fluent API表示兩表的主從關係

```csharp
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
   modelBuilder.Entity<TableA>().HasRequired(a => a.tableB).WithOptional(b => b.tablea);
}
```

之後更新回資料庫之後，查詢欄位定義，TableA的主鍵同時也是TableB外鍵

‌

# **一對多:**

兩個Model之間，一個包含集合屬性(母)，一個包含導覽屬性(子)，就會被視為一對多的關係。

假設一個員工都歸屬在一個部門，一個部門可包含多個員工。那可設計如下。

```csharp
public class Department
{
   public int DepartmentId { get; set; }
   public string DepartmentName { get; set; }
   public int? Manager { get; set; }
   public virtual ICollection<Employee> Employee { get; set; }   //Employee的集合屬性
}


public class Employee
{
   public int EmployeeId { get; set; }
   public string StaffName { get; set; }

   public int DepartmentId { get; set; }　　　　　　　　　　//Department的導覽屬性
   public virtual Department TheDepartment { get; set;}
}
```

針對Employee這個類別做一些說明，

- public virtual Department TheDepartment，有關Lazy loading的概念，假設我們抓出了一筆Employee資料，可以透過這個property把他的Department抓出來，但若沒有使用時這資料不會進入到記憶體中。
-  public int DepartmentId，此property代表Department的Key值，也可以用其他方式表達，如：DepartmentDepartmentId、TheDepartmentDepartmentId。明確的規則如下：
  - 目標Model的Key
  - 目標Model名稱  + 目標Model的Key，例如：DepartmentDepartmentId
  - 導覽屬性名稱　 + 目標Model的Key ，例如：TheDepartmentDepartmentId

‌

在子表中也可以透過屬性(ForeignKey)來設定外鍵

```csharp
public class Employee
{
   public int EmployeeId { get; set; }
   public string StaffName { get; set; }

   [Foreignkey("TheDepartment ")]
   public int DId { get; set; }　　　　　　　　　　
   public virtual Department TheDepartment { get; set;}
}

//或者

public class Employee
{
   public int EmployeeId { get; set; }
   public string StaffName { get; set; }

   public int DId { get; set; }　
　 [Foreignkey("DId")]
   public virtual Department TheDepartment { get; set;}
}
```

‌

# **多對多:**

在兩個類別之間，各自包含對應的導覽屬性，就會被當成多對多的關係。且會在資料庫中自動產生第三張表，紀錄兩張表的Key值。

```csharp
public class Student
{
   public int StudentId { get; set; }
   public string Name { get; set; }
   public string phone { get; set; }

   public ICollection<Course> Course { get; set; }   #包含Course的集合
}


public class Course
{
   public int Id { get; set; }
   public string CourseName { get; set; }
   public int Code { get; set; }

   public ICollection<Student> Students { get; set; }  #包含Student的集合
}
```

若依此直接執行 add-migration "Init"  -> update-database ，在資料庫當中除了會產生Student與Course兩張資料表之外，

還會產生一張名稱為 StudentCourse 的資料表，此表包含StudentId與CourseId兩個欄位，紀錄多對多關係。

也可以透過 Fluent API來改變，StudentCourse的表格以及欄位名稱。

```csharp
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
modelBuilder.Entity<Student>()
        .HasMany(x => x.Course)
        .WithMany(y => y.Students)
        .Map(x =>
            {
              x.ToTable("MyCourseAndStudent");
              x.MapLeftKey("StdId");
              x.MapRightKey("CouId");

        });
}
```

如上，調整後的表格名稱為"MyCourseAndStudent"，欄位名稱分別為"StdId"與"CouId"，去觀察欄位定義，雖然名稱不同但仍然會對應到Student與Course的Id。