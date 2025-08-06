import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { createCarForumArticle } from "../services/api/carForumArticleService";
import { getCarForumBrands } from "../services/api/carForumBrandService";
import { ROUTES } from "../utils/routes/routes";
import { useAuth } from "../contexts/AuthContext";

export function useCarForumCreate(resetForm) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const [brands, setBrands] = useState([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!isAuthenticated) {
      navigate(ROUTES.login);
    }
  }, [isAuthenticated, navigate]);

  useEffect(() => {
    getCarForumBrands()
      .then(setBrands)
      .catch((err) => {
        console.error("Failed to fetch brands:", err);
        setError("Unable to load brands");
      });
  }, []);

  const handleCreate = async (data) => {
    setLoading(true);
    setError("");

    try {
      const payload = {
        title: data.title,
        description: data.description,
        brandId: parseInt(data.brandId),
      };

      const created = await createCarForumArticle(payload);
      resetForm();
      navigate(`/car-forum/${created.id}`);
    } catch (err) {
      console.error(err);
      setError("Failed to create article. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return {
    brands,
    error,
    loading,
    handleCreate,
  };
}
