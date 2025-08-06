import { useNavigate } from "react-router-dom";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import useCarShopEdit from "../../hooks/useCarShopEdit";

export default function CarShopEditPage() {
  const navigate = useNavigate();
  const {
    formData,
    brands,
    handleChange,
    handleImageChange,
    handleSubmit,
    newImageFile,
  } = useCarShopEdit(navigate);

  if (!formData) return <div className="text-center p-10">Loading...</div>;

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader />

      <main className="flex-1 flex justify-center px-4 py-10">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-3xl bg-white border border-gray-300 shadow-lg rounded-xl p-8"
        >
          <p className="text-sm text-gray-500 mb-4">
            Updated at: {new Date(formData.updatedAt).toLocaleDateString()}
          </p>

          <label className="block text-sm font-medium mb-2">Current Image</label>
          <div className="relative w-full h-64 rounded-md overflow-hidden group mb-4 border border-gray-200">
            <img
              src={
                formData.imageUrl
                  ? `${import.meta.env.VITE_API_BASE_URL}${formData.imageUrl}`
                  : "https://via.placeholder.com/600x300"
              }
              alt="Current"
              className="w-full h-full object-cover"
            />
            <div className="absolute inset-0 bg-black/40 opacity-0 group-hover:opacity-100 transition flex items-center justify-center cursor-pointer">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-8 w-8 text-white"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M15.232 5.232l3.536 3.536M9 13h3l9-9a1.414 1.414 0 00-2-2l-9 9v3z"
                />
              </svg>
              <input
                type="file"
                accept="image/*"
                onChange={handleImageChange}
                className="absolute inset-0 opacity-0 cursor-pointer"
              />
            </div>
          </div>

          <div className="mt-6">
            <label className="block text-sm font-medium mb-1">Description</label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              className="w-full border rounded-md px-3 py-2 h-24"
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm">
            <div className="flex flex-col">
              <label className="mb-1 font-medium">Brand</label>
              <select
                name="brandId"
                value={formData.brandId}
                onChange={handleChange}
                className="border rounded-md px-3 py-2"
              >
                <option value="">Select Brand</option>
                {brands.map((brand) => (
                  <option key={brand.id} value={brand.id}>
                    {brand.name}
                  </option>
                ))}
              </select>
            </div>

            {[
              { name: "title", label: "Title" },
              { name: "model", label: "Model" },
              { name: "year", label: "Year", type: "number" },
              { name: "mileage", label: "Mileage", type: "number" },
              { name: "horsepower", label: "Horsepower", type: "number" },
              { name: "transmission", label: "Transmission" },
              { name: "fuelType", label: "Fuel Type" },
              { name: "color", label: "Color" },
              { name: "euroStandard", label: "Euro Standard" },
              { name: "doors", label: "Doors", type: "number" },
              { name: "price", label: "Price (€)", type: "number" },
              { name: "city", label: "City" },
              { name: "address", label: "Address" },
            ].map(({ name, label, type = "text" }) => (
              <div key={name} className="flex flex-col">
                <label className="mb-1 font-medium">{label}</label>
                <input
                  type={type}
                  name={name}
                  value={formData[name]}
                  onChange={handleChange}
                  className="border rounded-md px-3 py-2"
                />
              </div>
            ))}
          </div>

          <div className="flex justify-between mt-6">
            <button
              type="button"
              onClick={() => navigate(-1)}
              className="px-4 py-2 cursor-pointer bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
            >
              Save Changes
            </button>
          </div>
        </form>
      </main>

      <Footer />
    </div>
  );
}
