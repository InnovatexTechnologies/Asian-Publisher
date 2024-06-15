import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { REACT_APP_URL } from "../../../config/config";
import { MdOutlineRemoveShoppingCart } from "react-icons/md";
import {
  removeTocart,
  resetCart,
  updateTocart,
} from "../../../redux/slices/cartSlice";
import { toastError } from "../../../util/reactToastify";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import AsianLogoText from "../../../Images/AsianLogoText.jpeg";
import CartHeaderImage from "../../../Images/CartHeaderImage.png";
function Cart() {
  const { cartdata } = useSelector((state) => state.cart);
  const [cartItem, setCartItem] = useState([]);
  const [isButtonDisabled, setIsButtonDisabled] = useState(true);
  // console.log(" cart data loaded", cartdata);
  const dispatch = useDispatch();
  const deletecartItems = (book) => {
    const { id, ...rest } = book;
    // dispatch(resetCart());
    dispatch(removeTocart(id));
  };
  useEffect(() => {
    setCartItem(cartdata);
    setIsButtonDisabled(cartdata.length === 0);
  }, [cartdata]);

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
  const handleCheckoutClick = () => {
    if (cartItem.length === 0) {
      toastError("Please add items to the cart before proceeding to checkout.");
    } else {
      window.location.href = "/checkout";
    }
  };
  return (
    <>
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${CartHeaderImage})`,
          "background-size": "cover",
          "background-position": "center",
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
              "padding-top": "150px",
            }}
          >
            <p
              className="text"
              style={{
                "-webkit-text-transform": "uppercase",
                "text-transform": "uppercase",
                "font-size": "50px",
                "font-weight": "600",
                "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
                "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Cart
            </p>
            <p style={{ "font-size": "20px" }}>
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
              <table className="table table-spriped">
                <tbody>
                  <tr>
                    <th style={{ "font-size": "18px", width: "20%" }}>
                      Product Details
                    </th>
                    <th />
                    <th style={{ "font-size": "18px" }}>Qty</th>
                    <th
                      style={{
                        "-webkit-text-align": "right",
                        "text-align": "right",
                        "font-size": "18px",
                      }}
                    >
                      Price
                    </th>
                    <th
                      style={{
                        "-webkit-text-align": "right",
                        "text-align": "right",
                        "font-size": "18px",
                      }}
                    >
                      Sub Total
                    </th>
                    <th
                      style={{
                        "-webkit-text-align": "center",
                        "text-align": "center",
                        "font-size": "18px",
                      }}
                    >
                      Action
                    </th>
                  </tr>
                  {cartItem &&
                    cartItem.length > 0 &&
                    cartItem.map((book, index) => (
                      <tr index={index}>
                        <td>
                          <img
                            src={`${REACT_APP_URL}/Image/${book.image}`}
                            alt="Best Sellers"
                            loading="lazy"
                            style={{ height: "12vh" }}
                          />
                        </td>
                        <td
                          style={{ "font-weight": "600", "font-size": "18px" }}
                        >
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
                              if (val.length > 1 && val.startsWith('0')) {
                                val = val.replace(/^0+/, '');
                              }
                              if (!val || val < 1) {
                                val = 1;
                              }
                              updateOnChangeHandler(val, book);
                            }}
                          />
                        </td>
                        <td
                          style={{
                            "-webkit-text-align": "right",
                            "text-align": "right",
                          }}
                        >
                          Rs. {book.mRP}{" "}
                        </td>
                        <td
                          style={{
                            "-webkit-text-align": "right",
                            "text-align": "right",
                          }}
                        >
                          Rs.{book.quantity * book.mRP}{" "}
                        </td>
                        <td
                          style={{
                            "font-size": "25px",
                            "-webkit-text-align": "center",
                            "text-align": "center",
                          }}
                        >
                          <i
                            className="fa fa-trash"
                            aria-hidden="true"
                            style={{ "font-size": "20px", color: "#d82028" }}
                            onClick={() => {
                              deletecartItems(book);
                            }}
                          />
                        </td>
                      </tr>
                    ))}
                </tbody>
              </table>
              <center>
                <a
                   onClick={handleCheckoutClick}
                   disabled={isButtonDisabled}
                  style={{
                    "-webkit-text-decoration": "none",
                    "text-decoration": "none",
                    "background-color": "#d82028",
                    color: "#fff",
                    padding: "10px 50px 10px 50px",
                    "margin-top": "20px",
                    "border-radius": "10px",
                    cursor: isButtonDisabled ? "not-allowed" : "pointer",
                  }}
                >
                  CHECK OUT NOW
                </a>
              </center>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </>
  );
}

export default Cart;
