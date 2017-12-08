class SearchForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { nameSearch: "", nonProc: true };
    }
    handleChange(e) {
        var newObj = {};
        newObj[e.target.id] = e.target.value;
        this.setState(newObj);
    }
    handleCheckBoxChange(e) {
        var newObj = {};
        newObj[e.target.id] = e.target.checked;
        this.setState(newObj);
    }
    handleSubmit(e) {
        e.preventDefault();
        this.props.search(this.state);
    }

    render() {
        return (
            <div hidden={!this.props.vis} >
                <form className="form-inline" onSubmit={this.handleSubmit.bind(this)}>
                    <div className="form-group">
                        <label htmlFor="nameSearch">Name </label>
                        <input type="text" className="form-control" id="nameSearch" onChange={this.handleChange.bind(this)} value={this.state.nameSearch} />
                    </div> &nbsp;
                    <div className="checkbox">
                        <label>
                            <input type="checkbox" onChange={this.handleCheckBoxChange.bind(this)} id="nonProc" checked={this.state.nonProc}/> Non processed results only
                        </label>
                    </div> &nbsp;
                    <button type="submit" className="btn btn-success">
                        <span className="glyphicon glyphicon-search" aria-hidden="true"></span>
                        &nbsp;Search
                    </button>
                </form>
            </div>
            )          
    }
}

class PaginatorNumber extends React.Component {
    render() {
        return (
            <li><span onClick={() => this.props.goToPage(this.props.number)}>{this.props.number}</span></li>
            )
    }
}

class Paginator extends React.Component {
    render() {
        var pages = [];
        var strt = this.props.startAt;
        for (var i = 0; i < this.props.pages; i++) {
            if ((strt + i) == this.props.current) {
                pages.push(<li key={i} className="active"><span>{strt + i}<span className="sr-only">(current)</span></span></li>)
            } else {
                pages.push(<PaginatorNumber key={i} number={i + strt} goToPage={this.props.goToPage} />)
            }
        }

        return (
            <nav aria-label="Page navigation" className="dataTablePages" hidden={!this.props.vis}>
                <ul className="pagination">
                    <li className={this.props.current == this.props.startAt ? "disabled" : ""} onClick={this.props.decreasePage}>
                        <span aria-label="Previous" aria-hidden="true">&laquo;</span>
                    </li>
                    {pages}
                    <li className={this.props.current == (this.props.startAt + this.props.pages - 1) ? "disabled" : ""} onClick={this.props.increasePage}>
                        <span aria-label="Next" aria-hidden="true">&raquo;</span>
                    </li>
                </ul>
            </nav>
            )
    }
}

class DataTable extends React.Component {
    

    render() {
        return (
            <table className="table table-hover datatable">
                <thead>
                    <tr>
                        <th>Staff Name</th>
                        <th>Course</th>
                        <th>Score</th>
                        <th>Max Score</th>
                        <th>Result</th>
                        <th>Result Processed</th>
                    </tr>
                </thead>
                <tbody>
                {this.props.data.map(d => (
                        <tr key={d.Id} onClick={() => this.props.handleSelection(d.Id)}>
                            <td>{d.PersonName}</td>
                            <td>{d.CourseDesc}</td>
                            <td>{d.Score}</td>
                            <td>{d.MaxScore}</td>
                            <td>{d.PassFail ? "Passed" : "Failed"}</td>
                            <td>{d.Processed ? "Processed" : "Not Processed"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            )
    }
}

class EditForm extends React.Component {
    render() {
        return (
            <div id="editForm" className="modal fade" tabIndex="-1" role="dialog">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 className="modal-title">Edit Result</h4>
                        </div>
                        <div className="modal-body">
                            <dl className="dl-horizontal">
                                <dt>ID:</dt>
                                <dd>{this.props.record.Id}</dd>
                                <dt>Person ESR ID:</dt>
                                <dd>{this.props.record.PersonId}</dd>
                                <dt>Person name:</dt>
                                <dd>{this.props.record.PersonName}</dd>
                                <dt>Course ID:</dt>
                                <dd>{this.props.record.CourseId}</dd>
                                <dt>Course Description:</dt>
                                <dd>{this.props.record.CourseDesc}</dd>
                                <dt>Score:</dt>
                                <dd>{this.props.record.Score}</dd>
                                <dt>Max Score:</dt>
                                <dd>{this.props.record.MaxScore}</dd>
                                <dt>Passed or Failed:</dt>
                                <dd>{this.props.record.PassFail ? "Passed" : "Failed"}</dd>
                                <dt>Received:</dt>
                                <dd>{this.props.record.Received}</dd>
                                <dt>From AD Account:</dt>
                                <dd>{this.props.record.FromADAcc}</dd>
                                <dt>Processed:</dt>
                                <dd>{this.props.record.Processed ? "Processed" : "Not Processed"}</dd>
                                <dt>Comments:</dt>
                                <dd>{this.props.record.Comments}</dd>
                            </dl>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-default" data-dismiss="modal">Close</button>
                            <button type="button" className="btn btn-primary" disabled="disabled">Save changes</button>
                        </div>
                    </div>
                  </div>
                </div> 
            )
    }
}

class ELearnForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { };
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    ajaxSuccess(data) {
        $('#newres')[0].reset();
        this.props.newResult(data);
    }

    handleSubmit(e) {
        e.preventDefault();
        var ajaxSuccess = this.ajaxSuccess.bind(this);
        this.setState({
            PersonId: this.refs.personId.value,
            PersonName: this.refs.personName.value,
            CourseId: this.refs.courseId.value,
            CourseDesc: this.refs.courseDesc.value,
            Score: this.refs.score.value,
            MaxScore: this.refs.maxScore.value,
            PassFail: this.refs.passFail.value
        }, () => {
            $.ajax({
                type: "POST",
                url: "/api/ELResults",
                data: this.state,
                success: ajaxSuccess,
                error: function (data) {
                    alert("There was a problem");
                    console.log(data);
                }
            });
        });

    }

    render() {
        return (
            <form id="newres" className="form-horizontal" onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <label htmlFor="personId" className="col-sm-2 control-label" >Person ID</label>
                    <div className="col-sm-10">
                        <input name="personId" id="personId" ref="personId" className="form-control" placeholder="Please enter Staff ESR ID" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="personName" className="col-sm-2 control-label" >Person Name</label>
                    <div className="col-sm-10">
                        <input name="personName" id="personName" ref="personName" className="form-control large-field" placeholder="Please enter Staff Members Name" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="courseId" className="col-sm-2 control-label">Course ID</label>
                    <div className="col-sm-10">
                        <input name="courseId" id="courseId" ref="courseId" className="form-control" placeholder="Please enter course ID" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="courseDesc" className="col-sm-2 control-label">Course Description</label>
                    <div className="col-sm-10">
                        <input name="courseDesc" id="courseDesc" ref="courseDesc" className="form-control large-field" placeholder="Please enter course Name" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="score" className="col-sm-2 control-label">Score</label>
                    <div className="col-sm-10">
                        <input name="score" id="score" ref="score" className="form-control" type="number" placeholder="Please enter score" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="maxScore" className="col-sm-2 control-label">Maximum Score</label>
                    <div className="col-sm-10">
                        <input name="maxScore" id="maxScore" ref="maxScore" className="form-control" type="number" placeholder="Please enter maximum score" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="passFail" className="col-sm-2 control-label">Pass or Fail</label>
                    <div className="col-sm-10">
                        <select name="passFail" id="passFail" ref="passFail" className="form-control">
                            <option value="true">Pass</option>
                            <option value="false">Fail</option>
                        </select>
                    </div>
                </div>
                <button type="submit" className="btn btn-primary">Submit</button>
            </form>
            )
    }
}


class ELearningResultsApp extends React.Component {
    constructor(props) {
        super(props);
        this.state = { message: "Fetching Results...", data: [], selected: {}, showSearch: false, totalPages:0, currentPage: 1 };
        this.handleGoToPage = this.handleGoToPage.bind(this);
    }

    componentDidMount() {
        //this.setState({
        //    data: [{ id: 1, personName: "Stupid", courseDesc: "Paris 1", score: 10, maxScore: 10, passFail: true, processed: true },
        //        { id: 2, personName: "Idiot", courseDesc: "Paris 2", score: 5, maxScore: 10, passFail: false, processed: false }]
        //});
        //fetch('api/ELResults')
        //    .then(results => results.json())
        //    .then(items => this.setState({ data: items }));
        var ajaxSuccess = this.ajaxSuccess.bind(this);

        $.ajax({
            type: "GET",
            url: "api/ELResults?processed=false",
            success: ajaxSuccess,
            error: function (result) {
                alert('Error fetching results!');
                console.log(result);
            }
        });
        

    }

    ajaxSuccess(result) {
        var totPage = Math.floor((result.length - 1)/ this.props.pageSize) + 1;
        this.setState({ message: "E-Learning Results", data: result, totalPages: totPage, currentPage: 1, startAt: 1 });
    }

    handleResultSelection(id) {
        var result = this.state.data.filter(function (o) { return o.Id == id; });
        this.setState({ selected: result[0] });
        $('#editForm').modal('show');
    }

    toggleSearch() {
        this.setState({ showSearch: !this.state.showSearch });
    }

    handleSearch(s) {
        var queryString = "";
        if (s.nameSearch != "") {
            queryString += "?search=" + s.nameSearch;
        }
        if (s.nonProc) {
            queryString += (s.nameSearch == "") ? "?processed=false" : "&processed=false";
        }
        var ajaxSuccess = this.ajaxSuccess.bind(this);

        this.setState({ message: "Searching..." });

        $.ajax({
            type: "GET",
            url: "api/ELResults" + queryString,
            success: ajaxSuccess,
            error: function (result) {
                alert('Error fetching results!');
                console.log(result);
            }
        });
    }

    handleIncreasePage() {
        var newPage = this.state.currentPage >= this.state.totalPages ? this.state.totalPages : (this.state.currentPage + 1);
        this.handleGoToPage(newPage);
    }

    handleDecreasePage() {
        var newPage = this.state.currentPage <= 1 ? 1 : (this.state.currentPage - 1);
        this.handleGoToPage(newPage);
    }

    handleGoToPage(p) {
        var topPage = (p + 2) > this.state.totalPages ? this.state.totalPages : (p + 2);
        var startPage = (topPage - 4) < 1 ? 1 : (topPage - 4);
        this.setState({ currentPage: p, startAt: startPage });
    }

    handleNewResult(s) {
        var currentData = this.state.data;
        currentData.push(s);
        var totPage = Math.floor((currentData.length - 1) / this.props.pageSize) + 1;
        this.setState({ data: currentData, totalPages: totPage });
    }

    render() {
        return (
            <div>
                <h2>{this.state.message} <span id="searchIcon" className="glyphicon glyphicon-search" aria-hidden="true" onClick={this.toggleSearch.bind(this)}></span></h2>
                <SearchForm vis={this.state.showSearch} search={this.handleSearch.bind(this)}/>
                <DataTable data={this.state.totalPages > 1 ? this.state.data.slice(((this.state.currentPage - 1)*this.props.pageSize), (this.state.currentPage*this.props.pageSize)) : this.state.data} handleSelection={this.handleResultSelection.bind(this)} />
                <Paginator vis={this.state.totalPages > 1 ? true : false} pages={this.state.totalPages > 5 ? 5 : this.state.totalPages} startAt={this.state.startAt} current={this.state.currentPage} increasePage={this.handleIncreasePage.bind(this)} decreasePage={this.handleDecreasePage.bind(this)} goToPage={this.handleGoToPage}/>
                <EditForm record={this.state.selected}/>
                <hr />
                <h4>Manually Add Result:</h4>
                <ELearnForm newResult={this.handleNewResult.bind(this)}/>
            </div>
        );
    }
}

ReactDOM.render(
    <ELearningResultsApp pageSize={20} />,
    document.getElementById('app')
);
