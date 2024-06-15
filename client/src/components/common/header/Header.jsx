import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
// import "../../../css/Style.css";
// import "../../../css/bootstrap.min.css";
import AsianLogo from "../../../Images/AsianLogo.jpeg";
import catalog from "../../../Images/catalog.pdf";
import Sidebar from "../../common/sidebar/Sidebar";

function Header() {
  const navigate = useNavigate();
  const { quantity } = useSelector((state) => state.cart);
  const [myClass, setMyClass] = useState("search-box");
  const [search, setSearch] = useState("");

  function handleChange(event) {
    setSearch(event.target.value);
  }
  function handleSubmit(event) {
    event.preventDefault();
    navigate("/shop", {
      state: search,
    });
  }
  function handleLogout() {
    // Clear token from local storage
    localStorage.removeItem("token");
    localStorage.removeItem("userid");
    navigate("/login");
  }
  const token = localStorage.getItem("token");
  const userid = localStorage.getItem("userid");
  return (
    <>
    <Sidebar />
      <div className="navbar navbar-expand-lg bg-body-tertiary sticky-top">
        <div className="container-fluid">
          <div className="col-lg-1" style={{ zIndex: 1 }}>
            <a className="navbar-brand" href="#" style={{ color: "#fff" }}>
              <img
                src={AsianLogo}
                style={{ height: "10vh", borderRadius: 10 }}
              />
            </a>
          </div>
          <div className="col-lg-11">
            <div
              className="collapse navbar-collapse"
              id="navbarSupportedContent"
            >
              <center>
                <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link active"
                      aria-current="page"
                      href="/"
                      style={{ color: "#fff", fontSize: 16, fontWeight: 600 }}
                    >
                      HOME
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="/about-us"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      ABOUT US
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="/shop"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      SHOP
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="/author"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      AUTHORS
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="/orderForm"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      ORDER FORM
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="/becomean-author"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      BECOME AN AUTHOR
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      download
                      className="nav-link"
                      href={catalog}
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      OUR CATALOGUE
                    </a>
                  </li>
                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="/contact-us"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      CONTACT US
                    </a>
                  </li>
                  {token ? (
                    <li className="nav-item" style={{ zIndex: 1 }}>
                      <a
                        className="nav-link"
                        href={`/order-list/${userid}`}
                        style={{
                          color: "#fff",
                          fontSize: 16,
                          fontWeight: 600,
                          marginLeft: 10,
                        }}
                      >
                       HI USER
                      </a>
                    </li>
                  ) : (
                    <li className="nav-item" style={{ zIndex: 1 }}>
                      <a
                        className="nav-link"
                        href="/login"
                        style={{
                          color: "#fff",
                          fontSize: 16,
                          fontWeight: 600,
                          marginLeft: 10,
                        }}
                      >
                        LOGIN
                      </a>
                    </li>
                  )}
                  <li className="nav-item search-wrapper">
                    <a
                      className="nav-link"
                      href="#"
                      style={{ color: "#fff", fontSize: 16, fontWeight: 600 }}
                    >
                      <i
                        className="fa fa-search"
                        onClick={() => {
                          myClass === "search-box"
                            ? setMyClass("search-boxOpen")
                            : setMyClass("search-box");
                        }}
                      />
                    </a>
                    {/* Search Box search-box  */}
                    <div className={myClass}>
                      <div className="row">
                        <form onSubmit={handleSubmit}>
                          <input
                            type="text"
                            placeholder="Search..."
                            onChange={handleChange}
                          />
                          <button type="submit">
                            <i className="fa fa-search" />
                          </button>
                        </form>
                      </div>
                    </div>
                  </li>

                  <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      // href="#"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      <i
                        className="fa fa-shopping-cart"
                        onClick={() => navigate("/cart")}
                      >
                        <span
                          style={{
                            backgroundColor: "#fff",
                            borderRadius: "50%",
                            color: "#000",
                            fontSize: "8px",
                            padding: "5px",
                          }}
                        >
                          {quantity}
                        </span>
                      </i>
                    </a>
                  </li>
                  {/* <li className="nav-item" style={{ zIndex: 1 }}>
                    <a
                      className="nav-link"
                      href="#"
                      style={{
                        color: "#fff",
                        fontSize: 16,
                        fontWeight: 600,
                        marginLeft: 10,
                      }}
                    >
                      <i className="fa fa-user" />
                    </a>
                  </li> */}
                  {token ? (
                    <li className="nav-item" style={{ zIndex: 1 }}>
                      <a
                        className="nav-link"
                        href="#"
                        style={{
                          color: "#fff",
                          fontSize: 16,
                          fontWeight: 600,
                          marginLeft: 10,
                        }}
                        onClick={handleLogout} // Add onClick event to handle logout
                      >
                        <i className="fa fa-sign-out" />{" "}
                        {/* Assuming this icon is for logout */}
                      </a>
                    </li>
                  ) : null}
                </ul>
              </center>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default Header;
