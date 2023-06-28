# asp.net_mvc5_learnNotes
asp.net mvc5 亂寫一通學習筆記

使用 Visual Studio 2022 + 4.6.1 SDK
[參考之教學影片](https://www.youtube.com/watch?v=E7Voso411Vs)

- **<.NET 延伸模組(線上online搜尋)>** visual studio productivity power tools  (productivity power tools 20XX之類的) Visual Studio開發者必裝套件
- **<.NET 延伸模組(線上online搜尋)> Web Essentials** 增強 Visual Studio 在 Web、CSS、JavaScript開發上的方便性 !!!((暫時還沒有 Web Essentials 2022))
- **<.NET 延伸模組(線上online搜尋)> resharper** 程式碼編輯能力和即時錯誤顯示功能
- **Ctrl + Tab 在有使用到的資源文件中切換**
- 在 view中 **Ctrl + F5 (筆電可能要加Fn)** 可以直接在瀏覽器中執行view OR 重載入
- 瀏覽器中可以直接使用 **Ctrl + R 重載入頁面**
- 編輯完Controllor後，可以使用 **Ctrl + Shift + B 重新編譯，之後就可以到瀏覽器中 Ctrl + R 重載入頁面 測試路由**
- mvcaction4+tab 可以在Controller中創建Action範例
Code Snippet(mvcaction、prop、for):
[1. why-mvcaction4-code-snippet-does-not-react](https://stackoverflow.com/questions/42748460/why-mvcaction4-code-snippet-does-not-react "smartCard-inline")
[2. Will保哥](https://www.facebook.com/will.fans/posts/%E5%A6%82%E6%9E%9C%E6%9C%89%E4%BA%BA%E5%9C%A8-visual-studio-2017-%E4%B8%AD%E9%96%8B%E7%99%BC-aspnet-mvc%E4%BD%86%E5%8D%BB%E6%89%BE%E4%B8%8D%E5%88%B0-mvcaction4-%E6%88%96-mvcpostaction4-%E9%80%99%E5%85%A9%E5%80%8B%E4%B9%8B%E5%89%8D%E5%B8%B8%E7%94%A8/1721898351172635/ "smartCard-inline")https://stackoverflow.com/questions/42748460/why-mvcaction4-code-snippet-does-not-react/44009865#44009865
[3. ithelp](https://ithelp.ithome.com.tw/articles/10157797?sc=rss.iron "smartCard-inline")
    - prop+tab :

    ```
    public int MyProperty { get; set; }
    ```

    ‌

    - for+tab :

    ```
    for (int i = 0; i < length; i++){
    }
    ```

    ‌

    - mvcaction4+tab :

    ```
    public ActionResult Action(){
     return View();
    }
    ```
- **bootstrap 前端模板樣式資源:** [**Bootswatch**](https://bootswatch.com/ "‌")
  比如說要套用lumen主題，先在[**Bootswatch**](https://bootswatch.com/ "‌")中的Themes中找到Lumen，之後在Lumen下拉選單中找到他的bootstrap.css，可以改名為bootstrap-lumen.css以作區別，之後將其添加到專案資料夾的Content資料夾中。
  要替換bootstrap樣式時，可以到App_Start資料夾中的BundleConfig.cs，把bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", 中的bootstrap.css改名為bootstrap-lumen.css就可以套用成功，**Ctrl + F5** 重載套用
- Controllor 中依照回傳資料型態不同之 Action Results 路由命名規則:
  ![image.png](https://trello.com/1/cards/64992ae05908b708e981e0e4/attachments/6499361ad3f14c10c193ea6a/download/image.png)
- 進入類別或物件之原始檔中 **Ctrl + (Fn) + F12**
- ViewData: 控制器可以使用ViewData視圖數據字典傳遞數據給View前端視圖
  ```csharp
  var movie = new Movie() { Name = "Test123" };
  ViewData["Movie"] = movie; //ViewData方法專用
  ```
  1. 前端View(ViewData) :
  ```
  @using Pattern.Website.Models
  <h2>@( ((Movie) ViewData["Movie"]).Name )</h2>
  ```
  1. 另一種做法(@Model .) :
  ```
  @model Pattern.Website.Models.Movie
  <h2>@Model.Name</h2>
  ```
  1. 另一種做法(ViewBag.) :
  控制器
  ```
  ViewBag.Movie = movie;
  ```
  視圖
  ```
  @ViewBag.RandomMovie
  ```
  ‌
- ViewModel : 視圖模型，是專門為View視圖構建的模型，包含特定於View的任何數據和規則(Class會建立在ViewModels資料夾中，Class結尾要加上ViewModel字串)
  <Example>Models資料夾中已經有類別Movie.cs
  ```
  namespace Pattern.Website.Models
  {
      public class Movie
      {
          public int Id { get; set; }
          public string Name { get; set; }
      }
  }
  ```
  之後再加入Customer.cs類別
  ```
  namespace Pattern.Website.Models
  {
      public class Customer
      {
          public int Id { get; set; }
          public string Name { get; set; }
      }
  }
  ```
  再來在ViewModels資料夾中創建RandomMovieViewModel.cs類別
  ```
  using Pattern.Website.Models;

  namespace Pattern.Website.ViewModel
  {
      public class RandomMovieViewModel
      {
          public Movie Movie { get; set; }
          public List<Customer> Customers { get; set; }
      }
  }
  ```
  回到控制器中(上方using Pattern.Website.ViewModel;)
  ```
  ‌public ActionResult Random()
  {
      var movie = new Movie() { Name = "TestRandom123321" };
      var customers = new List<Customer>
      {
          new Customer { Name = "Customer 1" },
          new Customer { Name = "Customer 2" }
      };

      var viewModel = new RandomMovieViewModel
      {
          Movie = movie,
          Customers = customers
      };

      return View(viewModel);
  }
  ```
  回到前端View視圖中加入RandomMovieViewModel (上方加入@model Pattern.Website.ViewModel.RandomMovieViewModel)
  ```
  ‌<h2>@Model.Movie.Name</h2>
  @Model.Customers
  ```
- [ASP.NET](http://ASP.NET "‌") MVC 中的 Razor Views語法(將@Model .Customers遞迴印出):
  ```
  @{
      var className = Model.Customers.Count > 1 ? "popular" : null;
  }
  <h2 class="@className">@Model.Movie.Name</h2>
  @if (Model.Customers.Count == 0)
  {
      <text>No one has rented this movie before.</text>
  }
  else
  {
      <ul>
          @foreach (var customer in Model.Customers) @*將@model.customers跑迴圈印出*@

          {
              <li>@customer.Name</li>
          }
      </ul>
  }
  ```
- 前端Views中可以使用`@*註解內容*@`加入註解
- Partial Views部分視圖介紹 : 先從Views/Shared開啟_Layout.cshtml，這是網站整體的外觀引導模板，body裡面有個@RenderBody ()，這是渲染視圖的方法，<Partial Views部分視圖>就像是一個小視圖，可以在不同的視圖上重複調用
- Partial Views部分視圖創建方法，比如說要將\_Layout.cshtml主題中的導航欄布局提取到部分視圖中，可以在Views/Shared中創建一個Partial Views，先在Views/Shared資料夾按右鍵add->View，勾選Create as partial view，名稱前面需要添加\_這個下劃線，可以取名為\_NavBar，之後將\_Layout中所要提取出去的元素，滑鼠放在元素上面按住ctrl並按m兩次，使其折疊起來，之後就可以剪下貼到部分視圖_NavBar中。
  最後回到_Layout主體，在要渲染Partial部分視圖的地方加上@Html .Partial("_NavBar")這個渲染方法去渲染出Partial，也可以將Model傳遞給Partial View，可以在其中增加第二個參數(例如說要使用RandomMovieViewModel這個ViewModel中的Movie)，在Partial中加上@model Pattern.Website.ViewModel.RandomMovieViewModel引入此ViewModel
  ```
  @Html.Partial("_NavBar")
  ```
  改為
  ```
  @Html.Partial("_NavBar", Model.Movie)
  ```
- @Styles.Render以及@Scripts.Render：include一堆css以及一堆js(需先在~/App_Start/BundleConfig.cs做設定)
[**[ASP.NET MVC]@Styles.Render以及@Scripts.Render將include一堆css以及一堆js變簡單了，而且加快了行動版網頁的載入**](https://dotblogs.com.tw/kevinya/2015/10/30/153741)
需先到`~/App_Start/BundleConfig.cs`裡面做設定，bundles.add(OOXX)為例的話，就是將所有`~/Scripts/`資料夾裡面，所有版本的`jquery-ooxx.js`都加入一個bundle集合，並且把這個集合命名為`~/bundles/jquery`, 於是在.cshtml網頁中，如果想要include大量的jquery.js進來的話，只要寫一行程式碼`@Scripts.Render("~/bundles/jquery")`即可
    
- 使用icon字型(font awesome)：(Ex "fa fa-pencil-square-o"這個class`<h2><strong>@ViewBag.Title <i class="fa fa-pencil-square-o"></i></strong></h2>`)
Font Awesome 是一個基於CSS和LESS的字體和圖標工具套件。它由Dave Gandy製作，用於Twitter Bootstrap，後來被整合到BootstrapCDN 中。
    
- 
