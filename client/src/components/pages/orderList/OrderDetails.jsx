import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import { toastError } from "../../../util/reactToastify";
import Spinner from "../../common/Spinner";
import CartHeaderImage from "../../../Images/CartHeaderImage.png";
import Header from "../../common/header/Header";

function OrderDetails() {
  const location = useLocation();
  const user = location.state;
  const [loader, setLoader] = useState(true);
  const [orderDetails, setOrderDetails] = useState([]);

  useEffect(() => {
    async function OrderDetailsList() {
      setLoader(true);
      const token = localStorage.getItem("token");
      try {
        const response = await axios.get(
          `https://api.asianpublisher.in/api/OrderMetaApi`,
          {
            headers: {
              orderId: user?.id,
              Authorization: `Bearer ${token}`,
            },
          }
        );
        if (response?.data?.message === "Success") {
          setOrderDetails(response?.data?.orderMetas);
        }
      } catch (error) {
        toastError(error?.response?.data?.message);
      }
      setLoader(false);
    }
    OrderDetailsList();
  }, [user]);

  return (
    <>
      {loader && <Spinner />}
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${CartHeaderImage})`,
          "background-size": "cover",
          "background-position": "center",
          height: "40vh",
        }}
      >
        <div className="gradient-overlay" />
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
        <div className="col-lg-8" style={{ float: "left" }}>
          <div
            className="video-content"
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "padding-top": "120px",
            }}
          >
            <p
              className="text"
              style={{
                "-webkit-text-transform": "uppercase",
                "text-transform": "uppercase",
                "font-size": "40px",
                "font-weight": "600",
                "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
                "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Order Details
            </p>
          </div>
        </div>
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
      </div>
      <br />
      <div
        className="row"
        id="AboutUsSection"
        style={{
          display: "block",
          margin: "0px",
          padding: "0px",
          clear: "both",
        }}
      >
        <div className="container">
          <div className="row">
            <div className="col-lg-12 col-md-6">
              <div
                style={{
                  backgroundColor: "#eee",
                  padding: "10px 0px 10px 10px",
                }}
              >
                <p style={{ fontSize: "18px", padding: "0px", margin: "0px" }}>
                  <b>Name : </b> {user?.name} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <b>Mobile No. : </b> {user?.mobileNo}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <b>Order Date : </b> {user?.dateNew}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <b>Order Time : </b> {user?.timeNew}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <b>Status : </b> {user?.status ? "Paid" : "Unpaid"}
                </p>
              </div>
              <br></br>
              <table className="table table-spriped">
                <tr>
                  <th style={{ "font-size": "18px" }}>Name</th>
                  <th style={{ "font-size": "18px" }}>Quantity</th>
                  <th style={{ "font-size": "18px" }}>Price</th>
                  <th style={{ "font-size": "18px" }}>Sub Total</th>
                </tr>
                <tbody>
                  {orderDetails &&
                    orderDetails.length > 0 &&
                    orderDetails.map((order, index) => (
                      <tr index={index}>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {order?.bookNav?.name}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {order?.quantity}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {order?.price}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {order?.quantity && order?.price
                            ? order.quantity * order.price
                            : ""}
                        </td>
                      </tr>
                    ))}
                </tbody>
                <tfoot>
                  <tr>
                    <th style={{ "font-weight": "600", "font-size": "18px" }}>
                      Total
                    </th>
                    <th style={{ "font-weight": "600", "font-size": "18px" }}>
                      {orderDetails.reduce(
                        (total, order) => total + (order.quantity || 0),
                        0
                      )}
                    </th>
                    <th style={{ "font-weight": "600", "font-size": "18px" }}>
                      {orderDetails.reduce(
                        (total, order) => total + (order.price || 0),
                        0
                      )}
                    </th>
                    <th style={{ "font-weight": "600", "font-size": "18px" }}>
                      {orderDetails.reduce(
                        (total, order) =>
                          total + (order.quantity || 0) * (order.price || 0),
                        0
                      )}
                    </th>
                  </tr>
                </tfoot>
              </table>
            </div>
          </div>
        </div>
      </div>
      {/* <div
        className="row"
        style={{
          boxShadow: "0px -5px 5px rgba(0, 0, 0, 0.2)",
          paddingTop: 10,
          width: "100vw",
          bottom: "0px",
          position: "absolute",
        }}
      >
        <center>
          <p style={{ textAlign: "center", width: "100vw" }}>
            Designed &amp; Developed By Innovate X
          </p>
        </center>
      </div>{" "} */}
    </>
  );
}

export default OrderDetails;
