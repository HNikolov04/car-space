import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { getToken } from "../services/api/httpClient";
import { getCarShopBrands } from "../services/api/carShopBrandService";
import { createCarShopListing } from "../services/api/carShopListingService";

export default function useCarShopCreate() {
  const navigate = useNavigate();
  const [brands, setBrands] = useState([]);
  const { register, handleSubmit, reset } = useForm();

  useEffect(() => {
    if (!getToken()) {
      navigate("/login");
    }
  }, [navigate]);

  useEffect(() => {
    getCarShopBrands()
      .then(setBrands)
      .catch((err) => {
        console.error("Failed to fetch car brands:", err.message);
      });
  }, []);

  const onSubmit = async (data) => {
    try {
      const formData = {
        ...data,
        brandId: data.brandId === "" ? null : parseInt(data.brandId),
        year: parseInt(data.year),
        mileage: parseInt(data.mileage),
        horsepower: parseInt(data.horsepower),
        doors: parseInt(data.doors),
        price: parseFloat(data.price),
        imageFile: data.imageFile?.[0] || null,
      };

      const created = await createCarShopListing(formData);
      reset();
      navigate(`/car-shop/${created.id}`);
    } catch (err) {
      console.error("Failed to create car listing:", err.message);
    }
  };

  return {
    register,
    handleSubmit,
    onSubmit,
    brands,
  };
}
