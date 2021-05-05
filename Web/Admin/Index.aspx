<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Admin.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <h1>Hey there!</h1>
                <p class="text-large">This is a big 'ole hero unit, you can use this for calling extra attention to featured content or information.</p>
                <p>You could also add a call to action button. <em>(it's basically a jumbotron)</em></p>
                <a class="button">Call to action</a>
            </div>
        </div>
    </div>

    <!--Charts-->
    <h5>Charts</h5>
    <a class="anchor" name="charts"></a>
    <div class="row grid-responsive">
        <div class="column column-50">
            <div class="card">
                <div class="card-title">
                    <h2>Line Chart</h2>
                </div>
                <div class="card-block">
                    <div class="canvas-wrapper">
                        <canvas class="chart" id="line-chart" height="auto" width="auto"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="column column-50">
            <div class="card">
                <div class="card-title">
                    <h2>Bar Chart</h2>
                </div>
                <div class="card-block">
                    <div class="canvas-wrapper">
                        <canvas class="chart" id="bar-chart" height="auto" width="auto"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row grid-responsive">
        <div class="column column-33">
            <div class="card">
                <div class="card-title">
                    <h2>Pie Chart</h2>
                </div>
                <div class="card-block">
                    <div class="canvas-wrapper">
                        <canvas class="chart" id="pie-chart" height="auto" width="auto"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="column column-33">
            <div class="card">
                <div class="card-title">
                    <h2>Radar Chart</h2>
                </div>
                <div class="card-block">
                    <div class="canvas-wrapper">
                        <canvas class="chart" id="radar-chart" height="auto" width="auto"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="column column-33">
            <div class="card">
                <div class="card-title">
                    <h2>Polar Chart</h2>
                </div>
                <div class="card-block">
                    <div class="canvas-wrapper">
                        <canvas class="chart" id="polar-area-chart" height="auto" width="auto"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--Widgets-->
    <h5 class="mt-2">Widgets</h5>
    <a class="anchor" name="widgets"></a>
    <div class="row grid-responsive mt-1">
        <div class="column">
            <div class="card">
                <div class="card-title">
                    <h2 class="float-left">Notifications</h2>
                    <div class="badge background-primary float-right">36</div>
                    <div class="clearfix"></div>
                </div>
                <div class="card-block">
                    <div class="mt-1">
                        <img src="http://via.placeholder.com/45x45" alt="profile photo" class="circle float-left profile-photo" width="45" height="auto">
                        <div class="float-left ml-1">
                            <p class="m-0"><strong>Jane Donovan</strong> <span class="text-muted">uploaded a new photo</span></p>
                            <p class="text-small text-muted">30 minutes ago</p>
                        </div>
                        <div class="clearfix"></div>
                        <hr class="m-0 mb-2" />
                    </div>
                    <div class="mt-1">
                        <img src="http://via.placeholder.com/45x45" alt="profile photo" class="circle float-left profile-photo" width="45" height="auto">
                        <div class="float-left ml-1">
                            <p class="m-0"><strong>Sam Davidson</strong> <span class="text-muted">just replied to your message</span></p>
                            <p class="text-small text-muted">12 hours ago</p>
                        </div>
                        <div class="clearfix"></div>
                        <hr class="m-0 mb-2" />
                    </div>
                    <div class="mt-1">
                        <img src="http://via.placeholder.com/45x45" alt="profile photo" class="circle float-left profile-photo" width="45" height="auto">
                        <div class="float-left ml-1">
                            <p class="m-0"><strong>Kelly Johnson</strong> <span class="text-muted">changed her status</span></p>
                            <p class="text-small text-muted">2 days ago</p>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <div class="card-title">
                    <h2 class="float-left">Projects</h2>
                    <div class="badge float-right">3 In Progress</div>
                    <!--<div class="badge background-success float-right mr-1">5 Complete</div>-->
                    <div class="clearfix"></div>
                </div>
                <div class="card-block progress-bars">
                    <h5 class="float-left mt-1">Project Title</h5>
                    <p class="float-right text-small text-muted mt-1">25%</p>
                    <div class="clearfix"></div>
                    <div class="progress-bar">
                        <div class="progress background-primary" style="width: 25%;"></div>
                    </div>
                    <h5 class="float-left mt-1">Project Title</h5>
                    <p class="float-right text-small text-muted mt-1">50%</p>
                    <div class="clearfix"></div>
                    <div class="progress-bar">
                        <div class="progress background-primary" style="width: 50%;"></div>
                    </div>
                    <h5 class="float-left mt-1">Project Title</h5>
                    <p class="float-right text-small text-muted mt-1">75%</p>
                    <div class="clearfix"></div>
                    <div class="progress-bar">
                        <div class="progress background-primary" style="width: 75%;"></div>
                    </div>
                    <h5 class="float-left mt-1">Project Title</h5>
                    <p class="float-right text-small text-muted mt-1">100%</p>
                    <div class="clearfix"></div>
                    <div class="progress-bar">
                        <div class="progress background-primary" style="width: 100%;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--Forms-->
    <h5 class="mt-2">Forms</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Sample Form</h3>
                </div>
                <div class="card-block">
                    <form>
                        <fieldset>
                            <label for="nameField">Name</label>
                            <input type="text" placeholder="Jane Donovan" id="nameField">
                            <label for="ageRangeField">Age Range</label>
                            <select id="ageRangeField">
                                <option value="0-13">0-13</option>
                                <option value="14-17">14-17</option>
                                <option value="18-23">18-23</option>
                                <option value="24+">24+</option>
                            </select>
                            <label for="commentField">Comment</label>
                            <textarea placeholder="Hi Jane…" id="commentField"></textarea>
                            <div class="float-right">
                                <input type="checkbox" id="confirmField">
                                <label class="label-inline" for="confirmField">Send a copy to yourself</label>
                            </div>
                            <input class="button-primary" type="submit" value="Send">
                        </fieldset>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!--Alerts-->
    <h5 class="mt-2">Alerts</h5>
    <a class="anchor" name="alerts"></a>
    <div class="row grid-responsive">
        <div class="column">
            <div class="alert background-success"><em class="fa fa-thumbs-up"></em>Yay! Everything looks good</div>
            <div class="alert background-warning"><em class="fa fa-warning"></em>Are you sure you want to do that?</div>
            <div class="alert background-danger"><em class="fa fa-times-circle"></em>Oops, something went really wrong</div>
            <div class="alert"><em class="fa fa-bullhorn"></em>This is a generic alert without any defined background color</div>
        </div>
    </div>

    <!--Buttons-->
    <h5 class="mt-2">Buttons</h5>
    <a class="anchor" name="buttons"></a>
    <div class="row grid-responsive">
        <div class="column">
            <!-- Default Button -->
            <a class="button" href="#">Default Button</a>

            <!-- Outlined Button -->
            <button class="button button-outline">Outlined Button</button>

            <!-- Clear Button -->
            <input class="button button-clear" type="submit" value="Clear Button">
        </div>
    </div>

    <!--Tables-->
    <h5 class="mt-2">Tables</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Current Members</h3>
                </div>
                <div class="card-block">
                    <table>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Role</th>
                                <th>Age</th>
                                <th>Location</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Jane Donovan</td>
                                <td>UI Developer</td>
                                <td>23</td>
                                <td>Philadelphia, PA</td>
                            </tr>
                            <tr>
                                <td>Jonathan Smith</td>
                                <td>Designer</td>
                                <td>30</td>
                                <td>London, UK</td>
                            </tr>
                            <tr>
                                <td>Kelly Johnson</td>
                                <td>UX Developer</td>
                                <td>25</td>
                                <td>Los Angeles, CA</td>
                            </tr>
                            <tr>
                                <td>Sam Davidson</td>
                                <td>Programmer</td>
                                <td>28</td>
                                <td>Philadelphia, PA</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <!--Grid-->
    <h5 class="mt-2">Grid</h5>
    <a class="anchor" name="grid"></a>
    <div class="grid-example mt-2">
        <div class="row">
            <div class="column column-10">10%</div>
        </div>
        <div class="row">
            <div class="column column-20">20%</div>
        </div>
        <div class="row">
            <div class="column column-25">25%</div>
        </div>
        <div class="row">
            <!-- .column-33 or .column-34 both work for 1/3 column width -->
            <div class="column column-33">33%</div>
        </div>
        <div class="row">
            <div class="column column-40">40%</div>
        </div>
        <div class="row">
            <div class="column column-50">50%</div>
        </div>
        <div class="row">
            <div class="column column-60">60%</div>
        </div>
        <div class="row">
            <!-- .column-66 or .column-67 both work for 2/3 column width -->
            <div class="column column-67">67%</div>
        </div>
        <div class="row">
            <div class="column column-75">75%</div>
        </div>
        <div class="row">
            <div class="column column-80">80%</div>
        </div>
        <div class="row">
            <div class="column column-90">90%</div>
        </div>
        <div class="row">
            <div class="column">100%</div>
        </div>
    </div>
    <p class="credit">Trang quản trị -- <a href="https://www.medialoot.com">Phan Xuân Chánh</a></p>
</asp:Content>
