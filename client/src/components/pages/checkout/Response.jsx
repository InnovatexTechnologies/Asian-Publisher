import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./Response.css";
function Response() {
  const [isPrintMessage, setIsPrintMessage] = useState(false);
  const currentUrl = window.location.href;
  function getQueryParam(url, param) {
    const queryParams = new URLSearchParams(url.split("?")[1]);
    return queryParams.get(param);
  }
  const message = getQueryParam(currentUrl, "message");
  useEffect(() => {
    if (message === "SUCCESS") {
      setIsPrintMessage(true);
    }
  }, [currentUrl]);

  return (
    <div className="transaction-container">
      <div className="transaction-header">
        {isPrintMessage && (
          <div>
            <h3>Transaction Successfully</h3>
            <p>Our Team Will Contact You Soon</p>
          </div>
        )}
      </div>
      <center>
        <Link to={"/cart"}>
          <div className="transaction-footer">
            <button
              className="btn-back"
              style={{ zoom: "80%", backgroundColor: "red" }}
            >
              Back to Cart
            </button>
          </div>
        </Link>
      </center>
    </div>
  );
}

export default Response;
