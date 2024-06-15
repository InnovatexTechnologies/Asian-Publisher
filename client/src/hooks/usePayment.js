import { useState } from "react";
import axios from "axios";
import { toastError, toastSuceess } from "../util/reactToastify";

function usePayment() {
  const [paymentLoading, setPaymentLoading] = useState(false);
  //   const [error, setError] = useState(null);
  const [form, setForm] = useState({});

  async function getPaymentForm() {
    setPaymentLoading(true);
    // setError(null);
    try {
      const response = await axios.get(
        "https://api.asianpublisher.in/GateWay/Request"
        // {
        //   headers: {
        //     "Content-Type": "application/json",
        //   },
        // }
      );
      console.log("API  response", response);

      if (
        response?.data?.message !== undefined &&
        response?.data?.message === "Success"
      ) {
        toastSuceess("Your order has picked successfully");
        setForm(response);
        // toastSuceess(response?.data?.message);
      }
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
    setPaymentLoading(false);
  }

  return { paymentLoading, form, getPaymentForm };
}

export default usePayment;
