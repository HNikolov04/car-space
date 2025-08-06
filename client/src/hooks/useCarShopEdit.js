import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import {
  getCarShopListingById,
  updateCarShopListing,
} from "../services/api/carShopListingService";
import { getCarShopBrands } from "../services/api/carShopBrandService";

export default function useCarShopEdit(navigate) {
  const { id } = useParams();
  const [formData, setFormData] = useState(null);
  const [brands, setBrands] = useState([]);
  const [newImageFile, setNewImageFile] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const listing = await getCarShopListingById(id);
        const allBrands = await getCarShopBrands();

        setFormData({ ...listing, imageFile: null });
        setBrands(allBrands);
      } catch (err) {
        console.error("Failed to fetch data:", err);
        navigate("/car-shop");
      }
    };

    fetchData();
  }, [id, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e) => {
    setNewImageFile(e.target.files[0]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await updateCarShopListing({ ...formData, imageFile: newImageFile });
      navigate(`/car-shop/${id}`);
    } catch (err) {
      console.error("Failed to update listing:", err);
    }
  };

  return {
    formData,
    brands,
    newImageFile,
    handleChange,
    handleImageChange,
    handleSubmit,
  };
}
