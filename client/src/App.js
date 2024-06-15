import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./components/pages/home/Home";
import AboutUs from "./components/pages/aboutus/AboutUs";
import ContactUs from "./components/pages/contactus/ContactUs";
import Author from "./components/pages/author/Author";
import BecomeanAuthor from "./components/pages/becomeanAuthor/BecomeanAuthor";
import Login from "./components/common/login/Login";
import OrderForm from "./components/pages/orderForm/OrderForm";
import Register from "./components/common/register/Register";
import Shop from "./components/pages/shop/Shop";
import Cart from "./components/pages/cart/Cart";
import Checkout from "./components/pages/checkout/Checkout";
import BookDetails from "./components/pages/shop/BookDetails";
import Response from "./components/pages/checkout/Response";
import OrderList from "./components/pages/orderList/OrderList";
import OrderDetails from "./components/pages/orderList/OrderDetails";
import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { Outlet, useNavigate } from "react-router-dom";
import PrivacyPolicy from "./components/pages/privacypolicy/PrivacyPolicy";
import TermsandCondition from "./components/pages/termsandcondition/TermsandCondition";
import CancelationPolicy from "./components/pages/CancelationPolicy/CancelationPolicy";
// import Layout from "./components/Layout";
const  ProtectedRoute = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (localStorage.getItem("token")) {
        } else {
          navigate("/login");
        }
      } catch (error) {
        if (
          !error?.response?.data?.success &&
          error?.response?.data?.message === "Token Is Invalid"
        ) {
          localStorage.removeItem("token");
          const answer = window.confirm("Dear User your Token has expired !");
          if (answer) {
            navigate("/login");
          }
        }
      }
      return;
    };

    fetchData();
  }, [navigate, dispatch]);

  return <Outlet />;
};

const App = () => {
  const token = localStorage.getItem("token");
  return (
    <Router>
      {/* <Layout> */}
      <Routes>
        <Route path="/" element={<Home />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/order-list/:id" element={<OrderList />} />
          <Route path="/order-details" element={<OrderDetails />} />
        </Route>
        <Route path="/response" element={<Response />} />
        <Route path="/shop" element={<Shop />} />
        <Route path="/about-us" element={<AboutUs />} />
        <Route path="/orderForm" element={<OrderForm />} />
        <Route path="/becomean-author" element={<BecomeanAuthor />} />
        <Route path="/contact-us" element={<ContactUs />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/author" element={<Author />} />
        <Route path="/checkout" element={<Checkout />} />
        <Route path="/PrivacyPolicy" element={<PrivacyPolicy />} />
        <Route path="/TermsandCondition" element={<TermsandCondition />} />
        <Route path="/CancelationPolicy" element={<CancelationPolicy />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/bookdetails/:id" element={<BookDetails />} />
      </Routes>
      {/* </Layout> */}
    </Router>
  );
};

export default App;
