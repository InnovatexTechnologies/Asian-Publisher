import React, { useEffect, useState } from "react";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import { updateTocart } from "../../../redux/slices/cartSlice";
import { useDispatch, useSelector } from "react-redux";
import { REACT_APP_URL } from "../../../config/config";
import CartHeaderImage from "../../../Images/CartHeaderImage.png";
import axios from "axios";
import Spinner from "../../common/Spinner";
import { toastError, toastSuceess } from "../../../util/reactToastify";
import { resetCart } from "../../../redux/slices/cartSlice";

function Checkout() {
  const { cartdata } = useSelector((state) => state.cart);
  const dispatch = useDispatch();
  const [loader, setLoader] = useState(false);
  const [cartItem, setCartItem] = useState([]);
  const [totalQnt, setTotalQnt] = useState(0);
  const [totalPrice, setTotalPrice] = useState(0);
  const [totalAmmount, setTotalAmmount] = useState(0);
  const [options, setOptions] = useState({
    atomTokenId: "",
    merchId: "",
    custEmail: "",
    custMobile: "",
    returnUrl: "https://api.asianpublisher.in/GateWay/Response",
  });
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    mobileNo: "",
    address: "",
    city: "",
    state: "",
    country: "India",
    pincode: "",
  });
  function openPay() {
    window.AtomPaynetz(options, "uat");
  }
  useEffect(() => {
    if (
      options.atomTokenId !== "" &&
      options.merchId !== "" &&
      options.custEmail !== "" &&
      options.custMobile !== ""
    ) {
      openPay();
    }
  }, [options]);

  const cartdataitem = cartdata.map((cart) => ({
    bookId: cart.id,
    quantity: cart.quantity,
    price: cart.mRP,
  }));

  function handleChange(event) {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  }
  /**handle submit  */
  async function handleSubmit(event) {
    event.preventDefault();
    setLoader(true);
    try {
      const response1 = await axios.post(
        "https://api.asianpublisher.in/api/UserTokenApi",
        { ...formData },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      // console.log(response1.data)
      localStorage.setItem("token", response1.data.response);

      localStorage.setItem("userid", response1.data.userId);

      const response = await axios.post(
        "https://api.asianpublisher.in/api/OrderApi",
        { ...formData, orderMetas: cartdataitem },
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${response1.data.response}`,
          },
        }
      );
      //console.log("Token:", response.data.order.tokenId);
      if (
        response?.data?.order?.tokenId !== "" &&
        response?.data?.order?.merchId !== "" &&
        response?.data?.message === "Success"
      ) {
        toastSuceess("Your order has picked successfully");
        const newData = {
          atomTokenId: response?.data?.order?.tokenId,
          merchId: response?.data?.order?.merchId,
          custEmail: response?.data?.order?.email,
          custMobile: response?.data?.order?.mobileNo,
        };
        setOptions((prev) => ({ ...prev, ...newData }));

        // dispatch(resetCart());
      }
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
    setLoader(false);
    dispatch(resetCart());
  }
  useEffect(() => {
    setCartItem(cartdata);
    setTotalQnt(0);
    setTotalPrice(0);
    setTotalAmmount(0);
  }, [cartdata]);

  useEffect(() => {
    if (cartItem.length > 0) {
      let tempTotalQnt = 0;
      let tempTotalPrice = 0;
      let tempTotalAmmount = 0;
      cartItem?.map((book) => {
        tempTotalQnt += parseInt(book.quantity);
        tempTotalPrice += parseInt(book.mRP);
        tempTotalAmmount += parseInt(book.quantity) * parseInt(book.mRP);
      });
      setTotalQnt(tempTotalQnt);
      setTotalPrice(tempTotalPrice);
      setTotalAmmount(tempTotalAmmount);
    }
  }, [cartItem]);
  const updateOnChangeHandler = (newVal, currentObj) => {
    let updatedObj = { ...currentObj };
    updatedObj.quantity = newVal;

    const {
      id,
      authors,
      bookCode,
      courseSemesters,
      image,
      isFeatured,
      languageId,
      quantity,
      languageNav,
      mRP,
      numId,
      name,
      ...rest
    } = updatedObj;
    dispatch(
      updateTocart({
        product: {
          id,
          authors,
          bookCode,
          courseSemesters,
          image,
          isFeatured,
          languageId,
          languageNav,
          mRP,
          numId,
          name,
          quantity,
        },
      })
    );
  };

  return (
    <>
      {loader && <Spinner />}
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${CartHeaderImage})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
        }}
      >
        <div className="gradient-overlay" />
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
        <div className="col-lg-8" style={{ float: "left" }}>
          <div
            className="video-content"
            style={{ textAlign: "center", paddingTop: 150 }}
          >
            <p
              className="text"
              style={{
                textTransform: "uppercase",
                fontSize: 50,
                fontWeight: 600,
                textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Check Out
            </p>
            <p style={{ fontSize: 20 }}>
              Asian Publishers is your life long Learning Partner. We have
              empowered the growth of Students,Teachers, &amp; Professionals
              since 1981.
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
        style={{ display: "block", margin: 0, padding: 0, clear: "both" }}
      >
        <div className="container-fluid">
          <div className="row">
            <div className="col-lg-6 col-md-6">
              <div
                className="contact-from contact-shadow"
                style={{ marginLeft: 0 }}
              >
                <form
                  method="post"
                  onSubmit={handleSubmit}
                  action=""
                  id="contact-form"
                  acceptcharset="UTF-8"
                  className="contact-form"
                >
                  <input
                    type="hidden"
                    name="form_type"
                    defaultvalue="contact"
                  />
                  <input type="hidden" name="utf8" defaultvalue="✓" />
                  <div className="col-lg-12">
                    <input
                      type="text"
                      placeholder="Name"
                      className="form-controlCustomized"
                      // name="contact[name]"
                      // id="ContactFormName"
                      // defaultvalue=""
                      name="name"
                      required
                      value={formData.name}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-6"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="email"
                      placeholder="Email"
                      className="form-controlCustomized"
                      // name="contact[email]"
                      // id="ContactFormEmail"
                      // defaultvalue=""
                      name="email"
                      required
                      value={formData.email}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-6"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="text"
                      id="ContactFormSubject"
                      // name="contact[subject]"
                      placeholder="Mobile No."
                      //defaultvalue=""
                      className="form-controlCustomized"
                      name="mobileNo"
                      required
                      value={formData.mobileNo}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-12"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="text"
                      id="ContactFormSubject"
                      //name="contact[subject]"
                      placeholder="Address"
                      //defaultvalue=""
                      className="form-controlCustomized"
                      name="address"
                      required
                      value={formData.address}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-6"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="text"
                      id="ContactFormSubject"
                      //name="contact[subject]"
                      placeholder="City"
                      //defaultvalue=""
                      className="form-controlCustomized"
                      name="city"
                      required
                      value={formData.city}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-6"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="text"
                      id="ContactFormSubject"
                      //name="contact[subject]"
                      placeholder="State"
                      //defaultvalue=""
                      className="form-controlCustomized"
                      name="state"
                      required
                      value={formData.state}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-6"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="text"
                      id="ContactFormSubject"
                      //name="contact[subject]"
                      placeholder="Country"
                      //defaultvalue=""
                      className="form-controlCustomized"
                      name="country"
                      required
                      // value={formData.country}
                      value="India"
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-6"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <input
                      type="text"
                      id="ContactFormSubject"
                      //name="contact[subject]"
                      placeholder="Pin Code"
                      //defaultvalue=""
                      className="form-controlCustomized"
                      name="pincode"
                      required
                      value={formData.pincode}
                      onChange={handleChange}
                    />
                  </div>
                  <div
                    className="col-lg-12"
                    style={{ float: "left", marginTop: 20 }}
                  >
                    <label>
                      <input type="checkbox" required /> &nbsp;&nbsp; I have
                      accepted all terms and conditions
                    </label>
                  </div>

                  <center>
                    <button
                      type="submit"
                      style={{
                        textDecoration: "none",
                        backgroundColor: "#d82028",
                        color: "#fff",
                        padding: "10px 50px 10px 50px",
                        marginTop: 20,
                        borderRadius: 10,
                        border: "none",
                      }}
                    >
                      PLACE YOUR ORDER NOW
                    </button>
                  </center>
                </form>
              </div>
            </div>
            <div className="col-lg-6 col-md-6">
              <table className="table table-spriped">
                <tbody>
                  <tr>
                    <th style={{ fontSize: 18, width: "10%" }}>Product</th>
                    <th style={{ fontSize: 18, width: "30%" }}>Details</th>
                    <th style={{ fontSize: 16, width: "20%" }}>Qty</th>
                    <th
                      style={{ fontSize: 16, width: "20%", textAlign: "right" }}
                    >
                      Price
                    </th>
                    <th
                      style={{ fontSize: 16, width: "20%", textAlign: "right" }}
                    >
                      Sub Total
                    </th>
                  </tr>
                  {cartItem &&
                    cartItem.length > 0 &&
                    cartItem?.map((book, index) => (
                      <tr key={index}>
                        <td>
                          <img
                            src={`${REACT_APP_URL}/Image/${book.image}`}
                            alt="Best Sellers"
                            loading="lazy"
                            style={{ height: "12vh" }}
                          />
                        </td>
                        <td style={{ fontWeight: 600, fontSize: 18 }}>
                          {book.name}
                        </td>
                        <td>
                          <input
                            type="number"
                            className="form-control"
                            name="quantity"
                            min={1}
                            max={1000}
                            value={book.quantity || 1}
                            style={{ width: "100%" }}
                            onChange={(e) => {
                              let val = e.target.value;
                              if (val.length > 1 && val.startsWith("0")) {
                                val = val.replace(/^0+/, "");
                              }
                              if (!val || val < 1) {
                                val = 1;
                              }
                              updateOnChangeHandler(val, book);
                            }}
                          />
                        </td>

                        <td style={{ textAlign: "right" }}>Rs. {book.mRP}</td>
                        <td style={{ textAlign: "right" }}>
                          Rs. {book.quantity * book.mRP}
                        </td>
                      </tr>
                    ))}
                  <tr>
                    <th>Total</th>
                    <th></th>
                    <th style={{ textAlign: "right" }}>{totalQnt}</th>
                    <th style={{ textAlign: "right" }}>Rs. {totalPrice}</th>
                    <th style={{ textAlign: "right" }}>Rs. {totalAmmount}</th>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
      <div
        className="row"
        style={{
          clear: "both",
          height: "10vh",
          background: "linear-gradient(to top, rgb(216, 32, 40, 0.1), #fff)",
        }}
      />
      <div
        className="row"
        style={{
          backgroundColor: "white",
          position: "relative",
          padding: "50px 30px 0px 30px",
          textAlign: "left",
          color: "black",
        }}
      >
        <Footer />
      </div>
    </>
  );
}

export default Checkout;
