class DataTable extends React.Component {
    

    render() {
        rowClicked = function (id) {
            $('#editForm').modal('show');
        }

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
                        <tr key={d.Id} onClick={() => rowClicked(d.Id)}>
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
                            <p>One fine body&hellip;</p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-default" data-dismiss="modal">Close</button>
                            <button type="button" className="btn btn-primary">Save changes</button>
                        </div>
                    </div>
                  </div>
                </div> 
            )
    }
}

class ELearnForm extends React.Component {
    render() {
        return (
            <form id="newres" className="form-horizontal">
                <div className="form-group">
                    <label htmlFor="personid" className="col-sm-2 control-label">Person ID</label>
                    <div className="col-sm-10">
                        <input name="personid" id="personid" className="form-control" placeholder="Please enter Staff ESR ID"/>
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="personname" className="col-sm-2 control-label" >Person Name</label>
                    <div className="col-sm-10">
                        <input name="personname" id="personname" className="form-control" placeholder="Please enter Staff Members Name"/>
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="courseid" className="col-sm-2 control-label">Course ID</label>
                    <div className="col-sm-10">
                        <input name="courseid" id="courseid" className="form-control" placeholder="Please enter course ID"/>
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="coursedesc" className="col-sm-2 control-label">Course Description</label>
                    <div className="col-sm-10">
                        <input name="coursedesc" id="coursedesc" className="form-control" placeholder="Please enter course Name"/>
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="score" className="col-sm-2 control-label">Score</label>
                    <div className="col-sm-10">
                        <input name="score" id="score" className="form-control" type="number" placeholder="Please enter score"/>
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="maxscore" className="col-sm-2 control-label">Maximum Score</label>
                    <div className="col-sm-10">
                        <input name="maxscore" id="maxscore" className="form-control" type="number" placeholder="Please enter maximum score" />
                    </div>
                </div>
                <div className="form-group">
                    <label htmlFor="passfail" className="col-sm-2 control-label">Pass or Fail</label>
                    <div className="col-sm-10">
                        <select name="passfail" id="passfail" className="form-control">
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
        this.state = { message: "Fetching Results", data: [] };
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
            url: "api/ELResults",
            success: ajaxSuccess,
            error: function (result) {
                alert('Error fetching results!');
                console.log(result);
            }
        });
        

    }

    ajaxSuccess(result) {
        this.setState({ message: "E-Learning Results", data: result });
    }

    render() {
        return (
            <div>
                <h2>{this.state.message}</h2>
                <DataTable data={this.state.data} />
                <EditForm />
                <hr />
                <h4>Manually Add Result:</h4>
                <ELearnForm />
            </div>
        );
    }
}

ReactDOM.render(
    <ELearningResultsApp/>,
    document.getElementById('app')
);

//var CommentBox = React.createClass({
//    render: function () {
//        return (
//            <div className="commentBox">
//                Hello, world! I am a CommentBox.
//      </div>
//        );
//    }
//});
//ReactDOM.render(
//    <CommentBox />,
//    document.getElementById('app')
//);