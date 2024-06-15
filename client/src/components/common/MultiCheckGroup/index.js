import * as React from "react";
import Checkbox from "@mui/material/Checkbox";

export default function MultiCheckGroup({ data, setStateDispatch }) {
  const [checkedDataList, setCheckedDataList] = React.useState([]);

  const onChangeHandler = (e, newVal) => {
    if (e.target.checked) {
      const copyData = [...checkedDataList];
      copyData.push(newVal?.id);
      setCheckedDataList(copyData);
      setAllDataHandler(copyData);
    } else {
      const copyData = [...checkedDataList];
      const filterData = copyData.filter((item) => item !== newVal?.id);
      setCheckedDataList(filterData);
      setAllDataHandler(filterData);
    }
  };

  const setAllDataHandler = (newData) => {
    setStateDispatch(newData);
  };
  return (
    <div style={{ height: "100%" }}>
      {data?.length > 0 &&
        data.map((item) => (
          <>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                padding: "0 10px 0 10px",
              }}
            >
              <div style={{ display: "flex", alignItems: "center" }}>
                <div>
                  <Checkbox onChange={(e) => onChangeHandler(e, item)} />
                </div>
                <div>{item?.name} </div>
              </div>
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  background: "#f6f6f6",
                  minWidth: " 35px",
                  textAlign: "center",
                  borderRadius: "5px",
                  padding: "0 5px",
                }}
              >
                {item?.numId}
              </div>
            </div>
          </>
        ))}
    </div>
  );
}
