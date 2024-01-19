import React, { useEffect, useState } from "react";
import "./surveysicontool.css";
import debug from "sabio-debug";
import surveysService from "services/surveysService";
import { Table } from "react-bootstrap";
import TableRow from "./TableRow";
import "./surveys.css";
import { format } from "date-fns";
import Pagination from "rc-pagination";
import "rc-pagination/assets/index.css";
import SurveysIconTool from "./SurveysIconTool";

function Surveys() {
  const [data, setData] = useState([]);
  const [statusId, setStatusId] = useState("0");
  const [excluded, setExcluded] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalItems, setTotalCount] = useState();

  const pageSize = 6;

  const _logger = debug.extend("SurveysDash");

  const updateDataItem = (updatedItem) => {
    setData((prevState) => {
      const mapUpdatedData = (item) => {
        if (item.id === updatedItem.id) {
          const oldItem = { ...item };
          const newItem = { ...oldItem, ...updatedItem };
          return newItem;
        }
        return item;
      };

      const updatedState = prevState.map(mapUpdatedData);

      return updatedState;
    });
  };

  const filterData = (item) => {
    const inactive = 2;
    const allSurveys = "0";
    if (statusId === allSurveys) {
      return item.status.id !== inactive;
    } else {
      return item.status.id.toString() === statusId;
    }
  };

  const mapSingleItem = (item) => {
    const formattedDateModified = format(
      new Date(item.dateModified + "Z"),
      "MM/dd/yyyy hh:mm:ss a"
    );
    const formattedDateCreated = format(
      new Date(item.dateCreated + "Z"),
      "MM/dd/yyyy hh:mm:ss a"
    );
    return (
      <TableRow
        data={item}
        key={item.id}
        updateParentData={updateDataItem}
        fetchData={fetchData}
        setData={setData}
        formattedDateModified={formattedDateModified}
        formattedDateCreated={formattedDateCreated}
      />
    );
  };

  const mapItems = () => {
    const filteredData = data.filter(filterData);
    return filteredData.map(mapSingleItem);
  };

  const handleDropdownChange = (e) => {
    const selectedStatus = e.target.value;

    setStatusId(selectedStatus);
    setExcluded(selectedStatus === 2);
  };

  const fetchData = (status, exclude, page) => {
    const pageIndex = page - 1;

    const statusId = status;
    const excluded = exclude;

    surveysService
      .surveysGetAll(pageIndex, pageSize, statusId, excluded)
      .then(onSurveyGetSuccess)
      .catch(onSurveyGetError);
  };

  useEffect(() => {
    fetchData(statusId, excluded, currentPage);
  }, [statusId, excluded, currentPage]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const onSurveyGetSuccess = (response) => {
    _logger("onSurveyGetSuccess", response);

    setTotalCount(response.item.totalCount);
    setData(response.item.pagedItems);
  };

  const onSurveyGetError = (response) => {
    _logger("onSurveyGetError", response);
  };

  return (
    <React.Fragment>
      <div className="surveys-container surveys-flex-container">
        <SurveysIconTool />
        <p>
          <strong>Tip:</strong> Double click the name change the text and press
          enter to edit the name of a survey. Click the status/type to edit a
          surveys status and/or type. Click on the Survey Description to open a
          modal-view that shows the full survey name and full description. Use
          Escape to exit any editing menus/dropdowns. 😁
        </p>
        <div className="table-container" id="table">
          <Table responsive="sm" striped hover>
            <thead>
              <tr>
                <th scope="col left-name" className="text-head">
                  Name
                </th>
                <th scope="col" className="text-head">
                  Description
                </th>
                <th scope="col" className="text-head">
                  Status
                </th>
                <th scope="col" className="text-head">
                  Survey Type
                </th>
                <th scope="col right-name" className="text-head">
                  Date Modified
                </th>
                <th scope="col" className="text-head">
                  Date Created
                </th>
                <th scope="col" className="text-head">
                  <label htmlFor="surveyStatus" className="text-head">
                    Select Status:
                  </label>
                  <select
                    id="surveyStatus"
                    value={statusId}
                    onChange={handleDropdownChange}
                  >
                    <option value="0">All Surveys</option>
                    <option value="1">Active</option>
                    <option value="2">Inactive</option>
                    <option value="3">Pending</option>
                    <option value="4">Cancelled</option>
                  </select>
                </th>
              </tr>
            </thead>
            <tbody className="tbody-left-marg">{mapItems()}</tbody>
          </Table>
        </div>
        <Pagination
          current={currentPage}
          total={totalItems}
          pageSize={pageSize}
          onChange={handlePageChange}
        />
      </div>
    </React.Fragment>
  );
}

export default Surveys;
