<%@ Page Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.User.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="slider movie-items">
        <div class="container">
            <div class="row">
                <div class="social-link">
                    <p>Theo dõi chúng tôi: </p>
                    <a href="#"><i class="ion-social-facebook"></i></a>
                    <a href="#"><i class="ion-social-twitter"></i></a>
                    <a href="#"><i class="ion-social-googleplus"></i></a>
                    <a href="#"><i class="ion-social-youtube"></i></a>
                </div>
                <div class="slick-multiItemSlider">
                    <% for (int i = 0; i < 12; i++)
                        { %>
                    <div class="movie-item">
                        <div class="mv-img">
                            <a href="#">
                                <img src="images/uploads/slider4.jpg" alt="" width="285" height="437"></a>
                        </div>
                        <div class="title-in">
                            <div class="cate">
                                <span class="blue"><a href="#">Tên thể loại 1</a></span> <span class="orange"><a href="#">Tên thể loại 1</a></span>
                            </div>
                            <h6><a href="#">Phim <%=i %></a></h6>
                            <p><i class="ion-android-star"></i><span>7.4</span> /10</p>
                        </div>
                    </div>
                    <%} %>
                </div>
            </div>
        </div>
    </div>
    <div class="movie-items">
        <div class="container">
            <div class="row ipad-width">
                <div class="col-md-8">
                    <div class="title-hd">
                        <h2>Thể loại 1</h2>
                        <a href="#" class="viewall">Xem tất cả <i class="ion-ios-arrow-right"></i></a>
                    </div>
                    <div class="tabs">
                        <ul class="tab-links">
                            <li class="active"><a href="#tab1">Phổ biến nhất</a></li>
                            <li><a href="#tab2">Đánh giá cao</a></li>
                            <li><a href="#tab3">Xem nhiều nhất</a></li>
                        </ul>
                        <div class="tab-content">
                            <div id="tab1" class="tab active">
                                <div class="row">
                                    <div class="slick-multiItem">
                                        <% for (int i = 0; i < 8; i++)
                                        { %>
                                        <div class="slide-it">
                                            <div class="movie-item">
                                                <div class="mv-img">
                                                    <img src="images/uploads/mv-item1.jpg" alt="" width="185" height="284">
                                                </div>
                                                <div class="hvr-inner">
                                                    <a href="moviesingle.html">Read more <i class="ion-android-arrow-dropright"></i></a>
                                                </div>
                                                <div class="title-in">
                                                    <h6><a href="#">Interstellar</a></h6>
                                                    <p><i class="ion-android-star"></i><span>7.4</span> /10</p>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                            <div id="tab2" class="tab">
                                <div class="row">
                                    <div class="slick-multiItem">
                                        <% for (int i = 0; i < 8; i++)
                                        { %>
                                        <div class="slide-it">
                                            <div class="movie-item">
                                                <div class="mv-img">
                                                    <img src="images/uploads/mv-item1.jpg" alt="" width="185" height="284">
                                                </div>
                                                <div class="hvr-inner">
                                                    <a href="moviesingle.html">Read more <i class="ion-android-arrow-dropright"></i></a>
                                                </div>
                                                <div class="title-in">
                                                    <h6><a href="#">Interstellar</a></h6>
                                                    <p><i class="ion-android-star"></i><span>7.4</span> /10</p>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                            <div id="tab3" class="tab">
                                <div class="row">
                                    <div class="slick-multiItem">
                                        <% for (int i = 0; i < 8; i++)
                                        { %>
                                        <div class="slide-it">
                                            <div class="movie-item">
                                                <div class="mv-img">
                                                    <img src="images/uploads/mv-item1.jpg" alt="" width="185" height="284">
                                                </div>
                                                <div class="hvr-inner">
                                                    <a href="moviesingle.html">Read more <i class="ion-android-arrow-dropright"></i></a>
                                                </div>
                                                <div class="title-in">
                                                    <h6><a href="#">Interstellar</a></h6>
                                                    <p><i class="ion-android-star"></i><span>7.4</span> /10</p>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                            <div id="tab4" class="tab">
                                <div class="row">
                                    <div class="slick-multiItem">
                                        <% for (int i = 0; i < 8; i++)
                                        { %>
                                        <div class="slide-it">
                                            <div class="movie-item">
                                                <div class="mv-img">
                                                    <img src="images/uploads/mv-item1.jpg" alt="" width="185" height="284">
                                                </div>
                                                <div class="hvr-inner">
                                                    <a href="moviesingle.html">Read more <i class="ion-android-arrow-dropright"></i></a>
                                                </div>
                                                <div class="title-in">
                                                    <h6><a href="#">Interstellar</a></h6>
                                                    <p><i class="ion-android-star"></i><span>7.4</span> /10</p>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="sidebar">
                        <div class="ads">
                            <img src="images/uploads/ads1.png" alt="" width="336" height="296">
                        </div>
                        <div class="celebrities">
                            <h4 class="sb-title">Spotlight Celebrities</h4>
                            <div class="celeb-item">
                                <a href="#">
                                    <img src="images/uploads/ava1.jpg" alt="" width="70" height="70"></a>
                                <div class="celeb-author">
                                    <h6><a href="#">Samuel N. Jack</a></h6>
                                    <span>Actor</span>
                                </div>
                            </div>
                            <div class="celeb-item">
                                <a href="#">
                                    <img src="images/uploads/ava2.jpg" alt="" width="70" height="70"></a>
                                <div class="celeb-author">
                                    <h6><a href="#">Benjamin Carroll</a></h6>
                                    <span>Actor</span>
                                </div>
                            </div>
                            <div class="celeb-item">
                                <a href="#">
                                    <img src="images/uploads/ava3.jpg" alt="" width="70" height="70"></a>
                                <div class="celeb-author">
                                    <h6><a href="#">Beverly Griffin</a></h6>
                                    <span>Actor</span>
                                </div>
                            </div>
                            <div class="celeb-item">
                                <a href="#">
                                    <img src="images/uploads/ava4.jpg" alt="" width="70" height="70"></a>
                                <div class="celeb-author">
                                    <h6><a href="#">Justin Weaver</a></h6>
                                    <span>Actor</span>
                                </div>
                            </div>
                            <a href="#" class="btn">See all celebrities<i class="ion-ios-arrow-right"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- latest new v1 section-->
    <div class="latestnew">
        <div class="container">
            <div class="row ipad-width">
                <div class="col-md-8">
                    <div class="ads">
                        <img src="images/uploads/ads2.png" alt="" width="728" height="106">
                    </div>
                    <div class="title-hd">
                        <h2>Latest news</h2>
                    </div>
                    <div class="tabs">
                        <ul class="tab-links-3">
                            <li class="active"><a href="#tab31">#Movies </a></li>
                            <li><a href="#tab32">#TV Shows </a></li>
                            <li><a href="#tab33"># Celebs</a></li>
                        </ul>
                        <div class="tab-content">
                            <div id="tab31" class="tab active">
                                <div class="row">
                                    <div class="blog-item-style-1">
                                        <img src="images/uploads/blog-it1.jpg" alt="" width="170" height="250">
                                        <div class="blog-it-infor">
                                            <h3><a href="#">Brie Larson to play first female white house candidate Victoria Woodull in Amazon film</a></h3>
                                            <span class="time">13 hours ago</span>
                                            <p>Exclusive: <span>Amazon Studios </span>has acquired Victoria Woodhull, with Oscar winning Room star <span>Brie Larson</span> polsed to produce, and play the first female candidate for the presidency of the United States. Amazon bought it in a pitch package deal. <span>Ben Kopit</span>, who wrote the Warner Bros film <span>Libertine</span> that has...</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab32" class="tab">
                                <div class="row">
                                    <div class="blog-item-style-1">
                                        <img src="images/uploads/blog-it2.jpg" alt="" width="170" height="250">
                                        <div class="blog-it-infor">
                                            <h3><a href="#">Tab 2</a></h3>
                                            <span class="time">13 hours ago</span>
                                            <p>Exclusive: <span>Amazon Studios </span>has acquired Victoria Woodhull, with Oscar winning Room star <span>Brie Larson</span> polsed to produce, and play the first female candidate for the presidency of the United States. Amazon bought it in a pitch package deal. <span>Ben Kopit</span>, who wrote the Warner Bros film <span>Libertine</span> that has...</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab33" class="tab">
                                <div class="row">
                                    <div class="blog-item-style-1">
                                        <img src="images/uploads/blog-it1.jpg" alt="" width="170" height="250">
                                        <div class="blog-it-infor">
                                            <h3><a href="#">Tab 3</a></h3>
                                            <span class="time">13 hours ago</span>
                                            <p>Exclusive: <span>Amazon Studios </span>has acquired Victoria Woodhull, with Oscar winning Room star <span>Brie Larson</span> polsed to produce, and play the first female candidate for the presidency of the United States. Amazon bought it in a pitch package deal. <span>Ben Kopit</span>, who wrote the Warner Bros film <span>Libertine</span> that has...</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="morenew">
                        <div class="title-hd">
                            <h3>More news on Blockbuster</h3>
                            <a href="#" class="viewall">See all Movies news<i class="ion-ios-arrow-right"></i></a>
                        </div>
                        <div class="more-items">
                            <div class="left">
                                <div class="more-it">
                                    <h6><a href="#">Michael Shannon Frontrunner to play Cable in “Deadpool 2”</a></h6>
                                    <span class="time">13 hours ago</span>
                                </div>
                                <div class="more-it">
                                    <h6><a href="#">French cannibal horror “Raw” inspires L.A. theater to hand out “Barf Bags”</a></h6>

                                    <span class="time">13 hours ago</span>
                                </div>
                            </div>
                            <div class="right">
                                <div class="more-it">
                                    <h6><a href="#">Laura Dern in talks to join Justin Kelly’s biopic “JT Leroy”</a></h6>
                                    <span class="time">13 hours ago</span>
                                </div>
                                <div class="more-it">
                                    <h6><a href="#">China punishes more than 300 cinemas for box office cheating</a></h6>
                                    <span class="time">13 hours ago</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="sidebar">
                        <div class="sb-facebook sb-it">
                            <h4 class="sb-title">Find us on Facebook</h4>
                            <iframe src="" data-src="https://www.facebook.com/plugins/page.php?href=https%3A%2F%2Fwww.facebook.com%2Fhaintheme%2F%3Ffref%3Dts&tabs=timeline&width=300&height=315px&small_header=true&adapt_container_width=false&hide_cover=false&show_facepile=true&appId" width="300" height="315" style="border: none; overflow: hidden"></iframe>
                        </div>
                        <div class="sb-twitter sb-it">
                            <h4 class="sb-title">Tweet to us</h4>
                            <div class="slick-tw">
                                <div class="tweet item" id="599202861751410688">
                                </div>
                                <div class="tweet item" id="297462728598122498">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end of latest new v1 section-->
</asp:Content>
