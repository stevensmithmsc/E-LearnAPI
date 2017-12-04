class DataTable extends React.Component {
    

    render() {
        rowClicked = function (id) {
            //alert(id);
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
                        <tr id={d.id} onClick={rowClicked(d.id)}>
                            <td>{d.personName}</td>
                            <td>{d.courseDesc}</td>
                            <td>{d.score}</td>
                            <td>{d.maxScore}</td>
                            <td>{d.passFail ? "Passed" : "Failed"}</td>
                            <td>{d.processed ? "Processed" : "Not Processed"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            )
    }
}

class ELearnForm extends React.Component {
    render() {
        return (
            <form id="newres" className="form-horizontal">
                <div className="form-group">
                    <label for="personid" className="col-sm-2 control-label">Person ID</label>
                    <div className="col-sm-10">
                        <input name="personid" id="personid" className="form-control" placeholder="Please enter Staff ESR ID"/>
                    </div>
                </div>
                <div className="form-group">
                    <label for="personname" className="col-sm-2 control-label" >Person Name</label>
                    <div className="col-sm-10">
                        <input name="personname" id="personname" className="form-control" placeholder="Please enter Staff Members Name"/>
                    </div>
                </div>
                <div className="form-group">
                    <label for="courseid" className="col-sm-2 control-label">Course ID</label>
                    <div className="col-sm-10">
                        <input name="courseid" id="courseid" className="form-control" placeholder="Please enter course ID"/>
                    </div>
                </div>
                <div className="form-group">
                    <label for="coursedesc" className="col-sm-2 control-label">Course Description</label>
                    <div className="col-sm-10">
                        <input name="coursedesc" id="coursedesc" className="form-control" placeholder="Please enter course Name"/>
                    </div>
                </div>
                <div className="form-group">
                    <label for="score" className="col-sm-2 control-label">Score</label>
                    <div className="col-sm-10">
                        <input name="score" id="score" className="form-control" type="number" placeholder="Please enter score"/>
                    </div>
                </div>
                <div className="form-group">
                    <label for="maxscore" className="col-sm-2 control-label">Maximum Score</label>
                    <div className="col-sm-10">
                        <input name="maxscore" id="maxscore" className="form-control" type="number" placeholder="Please enter maximum score" />
                    </div>
                </div>
                <div className="form-group">
                    <label for="passfail" className="col-sm-2 control-label">Pass or Fail</label>
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
        this.state = { message: "Stupid Message", data: [] };
    }

    componentDidMount() {
        this.setState({
            data: [{ id: 1, personName: "Stupid", courseDesc: "Paris 1", score: 10, maxScore: 10, passFail: true, processed: true },
                { id: 2, personName: "Idiot", courseDesc: "Paris 2", score: 5, maxScore: 10, passFail: false, processed: false }]
        });
    }

    render() {
        return (
            <div>
                <h2>{this.state.message}</h2>
                <DataTable data={this.state.data}/>
                <hr />
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