﻿// This script uses react to produce an interface allowing users to view, update and delete E-Learning Results

// Search form stores search parameters in state object and passes them to search function when button is pressed.
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

//This class represents a number(page) with the Paginator component that is not the active page.
class PaginatorNumber extends React.Component {
    render() {
        return (
            <li><span onClick={() => this.props.goToPage(this.props.number)}>{this.props.number}</span></li>
            )
    }
}

//renders the paginator component
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

class PersonDisplay extends React.Component {
    render() {
        if (this.props.Name) 
            return (<span>{ this.props.Name }</span>)
        else if (this.props.Id > 0)
            return (<span>Employee: <i>{this.props.Id}</i></span>)
        else if (this.props.UserName)
            return (<i>{this.props.UserName}</i>)
        else 
            return (<i>Unknown</i>)
    }
}

//The component renders the main datatable.
class DataTable extends React.Component {
    

    render() {
        return (
            <table className="table table-hover datatable">
                <thead>
                    <tr>
                        <th>Staff Name</th>
                        <th>Course</th>
                        <th>Completion Date</th>
                        <th>Result Processed</th>
                        <th>Source</th>
                    </tr>
                </thead>
                <tbody>
                {this.props.data.map(d => (
                        <tr key={d.Id} onClick={() => this.props.handleSelection(d.Id)}>
                            <td><PersonDisplay Name={d.PersonName} Id={d.PersonId} UserName={d.UserName} /></td>
                            <td>{d.CourseDesc ? d.CourseDesc : d.ModuleName}</td>
                            <td>{(new Date(d.CompletionDate)).toLocaleDateString('en-GB')}</td>
                            <td>{d.Processed ? "Processed" : "Not Processed"}</td>
                            <td>{d.FromADAcc}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            )
    }
}

//This component renders the modal edit form which allows for editing and deleting of E-Learning results if user access level is sufficent.
class EditForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { comments: "", processed: false }
    }
    componentWillReceiveProps(props) {
        var commentField = (props.record.Comments) ? props.record.Comments : "";
        this.setState({ comments: commentField, processed: props.record.Processed });
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
    handleSave() {
        this.setState({ id: this.props.record.Id }, () => { this.props.editResult(this.state) });
        $('#editForm').modal('hide');
    }
    handleDelete() {
        if (confirm("Are you sure you want to delete this result?  This cannot be undone!")) {
            this.props.deleteResult(this.props.record.Id);
            $('#editForm').modal('hide');
        }
    }
    render() {
        return (
            <div id="editForm" className="modal fade" tabIndex="-1" role="dialog">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 className="modal-title">{this.props.accessLevel > 0 ? "Edit Result" : "View Result"}</h4>
                        </div>
                        <div className="modal-body">
                            <dl className="dl-horizontal">
                                <dt>ID:</dt>
                                <dd>{this.props.record.Id}</dd>
                                <dt>Person ESR ID:</dt>
                                <dd>{this.props.record.PersonId===-1?"Not Recorded":this.props.record.PersonId}</dd>
                                <dt>Person LMS Username:</dt>
                                <dd>{this.props.record.UserName}</dd>
                                <dt>Person name:</dt>
                                <dd>{this.props.record.PersonName}</dd>
                                <dt>E-Learning Module:</dt>
                                <dd>{this.props.record.ModuleName}</dd>
                                <dt>Course Description:</dt>
                                <dd>{this.props.record.CourseDesc}</dd>
                                <dt>Completion Date:</dt>
                                <dd>{(new Date(this.props.record.CompletionDate)).toLocaleDateString('en-GB')}</dd>                              
                                <dt>Received:</dt>
                                <dd>{(new Date(this.props.record.Received)).toLocaleString('en-GB')}</dd>
                                <dt>Source:</dt>
                                <dd>{this.props.record.FromADAcc}</dd>
                                <dt>Processed:</dt>
                                <dd>{this.props.accessLevel > 0 ? <div className="checkbox"><label><input type="checkbox" checked={this.state.processed} id="processed" onChange={this.handleCheckBoxChange.bind(this)} />{this.state.processed?"Processed":"Not Processed"}</label></div>
                                                                : (this.props.record.Processed ? "Processed" : "Not Processed")}</dd>
                                <dt>Comments:</dt>
                                <dd>{this.props.accessLevel > 0 ? <textarea className="form-control large-field" rows={3} value={this.state.comments} id="comments" onChange={this.handleChange.bind(this)} /> :this.props.record.Comments}</dd>
                            </dl>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-danger" disabled={this.props.accessLevel > 1 ? "" : "disabled"} onClick={this.handleDelete.bind(this)}>Delete</button>
                            <button type="button" className="btn btn-primary" disabled={this.props.accessLevel > 0 ? "" : "disabled"} onClick={this.handleSave.bind(this)}>Save changes</button>
                            <button type="button" className="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                  </div>
                </div> 
            )
    }
}

//This component handles the new E-Learning Result form and will send the new record to the api via ajax.
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
            Employee: this.refs.personId.value,
            UserName: this.refs.uName.value,
            ModuleName: this.refs.courseDesc.value,
            CompletionDate: this.refs.cDate.value
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
                        <input type="number" name="personId" id="personId" ref="personId" className="form-control" placeholder="Please enter Staff ESR ID" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="uName" className="col-sm-2 control-label" >Person LMS UserName</label>
                    <div className="col-sm-10">
                        <input type="email" name="uName" id="uName" ref="uName" className="form-control large-field" placeholder="Please enter Staff Members LMS UserName" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="courseDesc" className="col-sm-2 control-label">E-Learning Module</label>
                    <div className="col-sm-10">
                        <input type="text" name="courseDesc" id="courseDesc" ref="courseDesc" className="form-control large-field" placeholder="Please enter the name of the e-learning module" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="cDate" className="col-sm-2 control-label">Completion Date</label>
                    <div className="col-sm-10">
                        <input type="date" name="cDate" id="cDate" ref="cDate" className="form-control" placeholder="Please enter date module was completed" />
                    </div>
                </div>
                
                <button type="submit" className="btn btn-primary">Submit</button>
            </form>
            )
    }
}

//This is the main component for this application.  It get a list of E-Learning Results (using ajax)
// and handles interactions between the other components and the data.
class ELearningResultsApp extends React.Component {
    constructor(props) {
        super(props);
        this.state = { message: "Fetching Results...", data: [], selected: {}, showSearch: false, totalPages:0, currentPage: 1, accessLevel: 0 };
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

        var updateAccess = this.updateAccess.bind(this);
        $.ajax({
            type: "GET",
            url: "api/values",
            success: updateAccess,
            error: function (result) {
                alert('Error getting access level!');
                console.log(result);
            }
        });
    }

    ajaxSuccess(result) {
        var totPage = Math.floor((result.length - 1)/ this.props.pageSize) + 1;
        this.setState({ message: "E-Learning Results", data: result, totalPages: totPage, currentPage: 1, startAt: 1 });
    }

    updateAccess(result) {
        this.setState({ accessLevel: result });
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

    handleEditResult(s) {
        var getRecord = this.getRecord.bind(this);

        $.ajax({
            type: "PUT",
            url: "api/ELResults/" + s.id,
            data: s,
            success: function (result) {
                console.log(result);
                getRecord(s.id);
            },
            error: function (result) {
                alert('Error saving changes!');
                console.log(result);
            }
        });

        var prevData = this.state.data;
        var updatedIndex = prevData.map(item => item.Id).indexOf(s.id);
        prevData[updatedIndex].Processed = s.processed;
        prevData[updatedIndex].Comments = s.comments;
        this.setState({ data: prevData });       
    }

    getRecord(id) {
        console.log("fetching", id);
        var recRecord = this.recRecord.bind(this);

        $.ajax({
            type: "GET",
            url: "api/ELResults/" + id,
            success: recRecord,
            error: function (result) {
                alert('Error fetching updated record!');
                console.log(result);
            }
        });
    }

    recRecord(result) {
        console.log(result);
        var prevData = this.state.data;
        var updatedIndex = prevData.map(item => item.Id).indexOf(result.Id);
        prevData[updatedIndex].PersonName = result.PersonName;
        prevData[updatedIndex].CourseDesc = result.CourseDesc;
        prevData[updatedIndex].Processed = result.Processed;
        prevData[updatedIndex].Comments = result.Comments;
        this.setState({ data: prevData });
    }

    handleDeleteResult(id) {
        $.ajax({
            type: "DELETE",
            url: "api/ELResults/" + id,
            error: function (result) {
                alert('Error deleting record!');
                console.log(result);
            }
        });

        var prevData = this.state.data;
        var deleteIndex = prevData.map(item => item.Id).indexOf(id);
        if (deleteIndex !== -1) prevData.splice(deleteIndex, 1);
        var totPage = Math.floor((prevData.length - 1) / this.props.pageSize) + 1;
        this.setState({ data: prevData, totalPages: totPage });
    }

    render() {
        return (
            <div>
                <h2>{this.state.message} <span id="searchIcon" className="glyphicon glyphicon-search" aria-hidden="true" onClick={this.toggleSearch.bind(this)}></span></h2>
                <SearchForm vis={this.state.showSearch} search={this.handleSearch.bind(this)}/>
                <DataTable data={this.state.totalPages > 1 ? this.state.data.slice(((this.state.currentPage - 1)*this.props.pageSize), (this.state.currentPage*this.props.pageSize)) : this.state.data} handleSelection={this.handleResultSelection.bind(this)} />
                <Paginator vis={this.state.totalPages > 1 ? true : false} pages={this.state.totalPages > 5 ? 5 : this.state.totalPages} startAt={this.state.startAt} current={this.state.currentPage} increasePage={this.handleIncreasePage.bind(this)} decreasePage={this.handleDecreasePage.bind(this)} goToPage={this.handleGoToPage}/>
                <EditForm record={this.state.selected} accessLevel={this.state.accessLevel} editResult={this.handleEditResult.bind(this)} deleteResult={this.handleDeleteResult.bind(this)}/>
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
