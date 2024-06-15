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
// import Layout from "./components/Layout";

const App = () => {
  return (
    <Router>
      {/* <Layout> */}
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/order-list" element={<OrderList />} />
        <Route path="/order-details" element={<OrderDetails />} />
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
        <Route path="/cart" element={<Cart />} />
        <Route path="/bookdetails/:id" element={<BookDetails />} />
      </Routes>
      {/* </Layout> */}
    </Router>
  );
};

export default App;
