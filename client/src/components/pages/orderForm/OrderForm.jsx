import React, { useEffect, useState } from "react";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import { createOrderFrom } from "../../../redux/slices/orderFromSlice";
import { useDispatch, useSelector } from "react-redux";
import TextField from "@mui/material/TextField";
import Autocomplete from "@mui/material/Autocomplete";
import { fetchBooks } from "../../../redux/slices/bookSlice";
import orderformimage from "../../../Images/orderformimage.png";
import { toastError, toastSuceess } from "../../../util/reactToastify";
import { Button } from "@mui/material";
function OrderForm() {
  const { loading } = useSelector((state) => state.orderFrom);
  const { books } = useSelector((state) => state.book);
  const topBooks = books?.map((book) => ({ label: book.name, id: book.id }));
  const dispatch = useDispatch();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    address: "",
    city: "",
    bookId: null,
    quantity: 1,
    mobileNo: "",
    description: "",
  });
  useEffect(() => {
    dispatch(fetchBooks({}));
  }, [dispatch]);

  const {
    name,
    email,
    address,
    city,
    bookId,
    quantity,
    mobileNo,
    description,
  } = formData;
  console.log(formData);

  /**handle change  method implement here */
  function handleChange(event) {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  }

  /**handle submit  method implement here */
  function handleSubmit(event) {
    event.preventDefault();

    if (!bookId) {
      toastError("Please Fill the book");
      return;
    }
    if (quantity <= 0) {
      toastError("Quantity Always greater than zero");
      return;
    }

    /** create instance of Form data here */
    // let orderFrom = new FormData();

    // orderFrom.append("name", name);
    // orderFrom.append("email", email);
    // orderFrom.append("address", address);
    // orderFrom.append("city", city);
    // orderFrom.append("bookId", book);
    // orderFrom.append("quantity", parseInt(quantity));
    // orderFrom.append("mobileNo", mobileNo);
    // orderFrom.append("description", description);

    /** hitt the create order from api from  here */
    let payload = { ...formData };
    payload.bookId = bookId?.id ? payload.bookId?.id : "";
    payload.quantity = parseInt(payload.quantity);

    dispatch(createOrderFrom(payload));
  }
  useEffect(() => {
    if (loading === "fulfilled") {
      setFormData({
        name: "",
        email: "",
        address: "",
        city: "",
        bookId: null,
        quantity: 1,
        mobileNo: "",
        description: "",
      });
    }
  }, [loading]);

  return (
    <>
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${orderformimage})`,
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
              Order Form
            </p>
            <p style={{ fontSize: 20 }}>
              Asian Publishers is your life long Learning Partner. We have
              empowered the growth of Students,Teachers, &amp; Professionals
              since 1981.
            </p>
            {}
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
        <form onSubmit={handleSubmit}>
          <div className="container">
            <div className="col-lg-6" style={{ float: "left" }}>
              <label style={{ fontWeight: 600 }}>Name</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Name"
                name="name"
                value={formData.name}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-6" style={{ float: "left" }}>
              <label style={{ fontWeight: 600 }}>Email</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Email"
                name="email"
                value={formData.email}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-6" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Address</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Enter your Address"
                name="address"
                value={formData.address}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-6" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>City</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Enter Your City"
                name="city"
                value={formData.city}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-12" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Books</label>
              <Autocomplete
                disablePortal
                id="combo-box-demo"
                value={formData.bookId}
                options={topBooks}
                onChange={(event, value) => {
                  setFormData((prev) => ({
                    ...prev,
                    bookId: value,
                  }));
                }}
                sx={{
                  ".MuiAutocomplete-inputRoot": {
                    "& .MuiAutocomplete-input": {
                      fontSize: "16px",
                    },
                  },
                }}
                renderInput={(params) => (
                  <TextField {...params} label="Books" />
                )}
              />
            </div>
            <div className="col-lg-6" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Quantity</label>
              <input
                className="form-controlCustomized"
                type="number"
                min={1}
                name="quantity"
                value={formData.quantity}
                required
                onChange={handleChange}
                placeholder="Quantity"
              />
            </div>
            <div className="col-lg-6" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Mobile No.</label>
              <input
                className="form-controlCustomized"
                type="text"
                name="mobileNo"
                value={formData.mobileNo}
                required
                onChange={handleChange}
                placeholder="Mobile No."
              />
            </div>
            <div className="col-lg-12" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Description</label>
              <textarea
                rows={5}
                className="form-control"
                name="description"
                value={formData.description}
                required
                onChange={handleChange}
              />
            </div>
            <div className="col-lg-12" style={{ float: "left", marginTop: 40 }}>
              <center>
                <Button
                  // href="#"
                  type="submit"
                  style={{
                    textDecoration: "none",
                    backgroundColor: "#d82028",
                    color: "#fff",
                    padding: "10px 50px 10px 50px",
                    marginTop: 20,
                    borderRadius: 10,
                  }}
                >
                  Order Now
                </Button>
              </center>
            </div>
          </div>
        </form>
      </div>
      <Footer />
    </>
  );
}

export default OrderForm;
