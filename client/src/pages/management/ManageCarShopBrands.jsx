import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import MainPageHeader from "../../components/shared/MainPageHeader";
import Footer from "../../components/shared/Footer";
import {
  getCarShopBrands,
  createCarShopBrand,
  updateCarShopBrand,
  deleteCarShopBrand,
} from "../../services/api/carShopBrandService";
import { useAuth } from "../../contexts/AuthContext";

export default function ManageCarShopBrands() {
  const navigate = useNavigate();
  const { user } = useAuth();

  const [brands, setBrands] = useState([]);
  const [formName, setFormName] = useState("");
  const [selectedBrandId, setSelectedBrandId] = useState(null);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!user || user.role !== "Administrator") {
      navigate("/");
    }
  }, [user, navigate]);

  useEffect(() => {
    loadBrands();
  }, []);

  const loadBrands = async () => {
    try {
      const data = await getCarShopBrands();
      setBrands(data || []);
    } catch (err) {
      console.error("Failed to fetch car shop brands:", err);
      setError("Failed to load car shop brands");
    }
  };

  const handleSubmit = async () => {
    if (!formName.trim()) return;

    try {
      if (selectedBrandId !== null) {
        await updateCarShopBrand(selectedBrandId, { id: selectedBrandId, name: formName });
      } else {
        await createCarShopBrand({ name: formName });
      }
      setFormName("");
      setSelectedBrandId(null);
      await loadBrands();
    } catch (err) {
      console.error("Error saving brand:", err);
    }
  };

  const handleEdit = (brand) => {
    setFormName(brand.name);
    setSelectedBrandId(brand.id);
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this brand?")) return;
    try {
      await deleteCarShopBrand(id);
      await loadBrands();
    } catch (err) {
      console.error("Delete failed:", err);
    }
  };

  const handleCancelEdit = () => {
    setFormName("");
    setSelectedBrandId(null);
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <MainPageHeader />

      <main className="flex-1 px-4 py-10 flex justify-center">
        <div className="w-full max-w-3xl bg-white rounded-xl shadow-lg border border-gray-300 p-8">
          <h2 className="text-2xl font-semibold mb-6 text-center">
            Manage Car Shop Brands
          </h2>

          <div className="flex flex-col sm:flex-row gap-3 mb-6">
            <input
              type="text"
              placeholder="Brand Name"
              className="flex-1 px-3 py-2 border rounded-md"
              value={formName}
              onChange={(e) => setFormName(e.target.value)}
            />
            <div className="flex gap-2">
              <button
                onClick={handleSubmit}
                className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
              >
                {selectedBrandId !== null ? "Update" : "Add"}
              </button>
              {selectedBrandId !== null && (
                <button
                  onClick={handleCancelEdit}
                  className="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-md shadow"
                >
                  Cancel
                </button>
              )}
            </div>
          </div>

          <div className="overflow-x-auto">
            <table className="w-full border border-gray-300 text-sm">
              <thead className="bg-gray-100 text-left">
                <tr>
                  <th className="border px-3 py-2 w-16">ID</th>
                  <th className="border px-3 py-2">Name</th>
                  <th className="border px-3 py-2 w-40">Actions</th>
                </tr>
              </thead>
              <tbody>
                {brands.map((brand) => (
                  <tr key={brand.id} className="hover:bg-gray-50">
                    <td className="border px-3 py-2">{brand.id}</td>
                    <td className="border px-3 py-2">{brand.name}</td>
                    <td className="border px-3 py-2 space-x-2">
                      <button
                        onClick={() => handleEdit(brand)}
                        className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded shadow"
                      >
                        Edit
                      </button>
                      <button
                        onClick={() => handleDelete(brand.id)}
                        className="bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded shadow"
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
                {brands.length === 0 && (
                  <tr>
                    <td colSpan="3" className="text-center text-gray-500 py-4 italic">
                      No brands available.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>

          <div className="mt-8 text-center">
            <button
              onClick={() => navigate("/management")}
              className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
            >
              Back to Management
            </button>
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
