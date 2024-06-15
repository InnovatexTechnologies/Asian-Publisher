import React, { useEffect, useState } from "react";
import axios from "axios";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { toastError, toastSuceess } from "../../../util/reactToastify";
import CartHeaderImage from "../../../Images/CartHeaderImage.png";
import Spinner from "../../common/Spinner";
import Header from "../../common/header/Header";
function OrderList() {
  const navigate = useNavigate();
  const id = useParams();
  // console.log(id)
  // const location = useLocation();
  // const userId = location.state;
  const [userOrder, setUserOrder] = useState([]);
  const [loader, setLoader] = useState(true);

  useEffect(() => {
    async function OrderList() {
      setLoader(true);
      const token = localStorage.getItem("token");
      const userid = localStorage.getItem("userid");
      try {
        const response = await axios.get(
          `https://api.asianpublisher.in/api/OrderApi/OrderListByUser`,
          {
            headers: {
              userId: userid,
              Authorization: `Bearer ${token}`,
            },
          }
        );
        console.log(response)
        if (response?.data?.message === "Success") {
          setUserOrder(response?.data?.orders);
        }
      } catch (error) {
        toastError(error?.response?.data?.message);
      }
      setLoader(false);
    }
    OrderList();
  }, [id?.id]);

  const cancelOrder = async (orderId) => {
    // Show confirmation alert
    const isConfirmed = window.confirm("Are you sure you want to cancel this order?");
    
    if (!isConfirmed) {
      return; // Exit if the user cancels the confirmation dialog
    }
  
    const token = localStorage.getItem("token");
    const userid = localStorage.getItem("userid");
  
    try {
      const response = await axios.put(
        `https://api.asianpublisher.in/api/OrderApi?id=${orderId}`,
        {},
        {
          headers: {
            userId: userid,
            Authorization: `Bearer ${token}`,
          },
        }
      );
  
      if (response?.data?.message === "Success") {
        toastSuceess("Order canceled successfully");
        setUserOrder(userOrder.filter((order) => order.id !== orderId));
        window.location.reload();
      } else {
        toastError(response?.data?.message);
        window.location.reload();
      }
    } catch (error) {
      toastError(error?.response?.data?.message);
      window.location.reload();
    }
  };
  
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
              Order List
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
              <table className="table table-spriped" style={{ zoom: "80%" }}>
                <tbody>
                  <tr>
                    <th style={{ "font-size": "18px", width: "10%" }}>
                      Order Id
                    </th>
                    <th style={{ "font-size": "18px", width: "20%" }}>Name</th>

                    <th style={{ "font-size": "18px" }}>Mobile No.</th>
                    <th style={{ "font-size": "18px" }}>Order Date</th>
                    <th style={{ "font-size": "18px" }}>Order Time</th>
                    <th
                      style={{
                        "font-size": "18px",
                      }}
                    >
                      Payment Status
                    </th>
                    <th
                      style={{
                        "font-size": "18px",
                      }}
                    >
                      Order Staus
                    </th>
                    <th
                      style={{
                        "font-size": "18px",
                        textAlign: "center",
                      }}
                    >
                      Action
                    </th>
                  </tr>
                  {userOrder &&
                    userOrder.length > 0 &&
                    userOrder.map((user, index) => (
                      <tr
                        index={index}
                        style={{
                          backgroundColor:
                            Number(user.isDispatch) === -1
                              ? "lightcoral"
                              : "inherit",
                        }}
                      >
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {user?.id}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {user?.name}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {user?.mobileNo}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {user?.dateNew}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {user?.timeNew}
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
                          {user?.status ? "Paid" : "Unpaid"}
                        </td>
                        <td style={{ fontWeight: "600", fontSize: "18px" }}>
                          {console.log(Number(user.isDispatch))}
                          {Number(user.isDispatch) === 1
                            ? "Dispatched"
                            : Number(user.isDispatch) === 0
                            ? "Shipped"
                            : user.isDispatch === -1
                            ? "Canceled"
                            : ""}
                        </td>

                        <td>
                          <center>
                            <button
                              style={{
                                backgroundColor: "green",
                                color: "#fff",
                                border: "none",
                                padding: "7px 15px 7px 15px",
                              }}
                              onClick={() => {
                                navigate("/order-details", { state: user });
                              }}
                            >
                              View Details
                            </button>
                            {Number(user.isDispatch) !== -1 && Number(user.isDispatch) !== 1 &&(
                              <>
                                &nbsp;
                                <button
                                  style={{
                                    backgroundColor: "red",
                                    color: "#fff",
                                    border: "none",
                                    padding: "7px 15px 7px 15px",
                                  }}
                                  onClick={() => cancelOrder(user?.id)}
                                >
                                  Cancel Order
                                </button>
                              </>
                            )}
                          </center>
                        </td>
                      </tr>
                    ))}
                </tbody>
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
          height:"10vh",
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

export default OrderList;
