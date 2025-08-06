import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import useCarMeetEdit from "../../hooks/useCarMeetEdit";

export default function CarMeetEditPage() {
  const {
    formData,
    newImageFile,
    loading,
    handleChange,
    handleImageChange,
    handleSubmit,
    handleCancel,
  } = useCarMeetEdit();

  if (loading || !formData) {
    return <div className="text-center py-10">Loading...</div>;
  }

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader />

      <main className="flex-1 flex justify-center px-4 py-10">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-3xl bg-white shadow-lg border border-gray-300 rounded-xl p-8 space-y-5"
        >
          <p className="text-sm text-gray-500">
            Updated at: {new Date(formData.updatedAt).toLocaleDateString()}
          </p>

          <label className="block text-sm font-medium">Current Image</label>
          <div className="relative w-full h-64 rounded-md overflow-hidden group mb-2 border border-gray-200">
            <img
              src={
                newImageFile
                  ? URL.createObjectURL(newImageFile)
                  : formData.imageUrl
                  ? `${import.meta.env.VITE_API_BASE_URL}${formData.imageUrl}`
                  : "https://via.placeholder.com/600x300"
              }
              alt="Meet Preview"
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

          {[
            { name: "title", label: "Title" },
            { name: "description", label: "Description", type: "textarea" },
            { name: "city", label: "City" },
            { name: "address", label: "Address" },
            { name: "meetDate", label: "Meet Date", type: "date" },
          ].map(({ name, label, type = "text" }) => (
            <div key={name}>
              <label className="block text-sm font-medium mb-1">{label}</label>
              {type === "textarea" ? (
                <textarea
                  name={name}
                  value={formData[name]}
                  onChange={handleChange}
                  className="w-full border rounded-md px-3 py-2 h-24"
                  required
                />
              ) : (
                <input
                  name={name}
                  type={type}
                  value={formData[name]}
                  onChange={handleChange}
                  className="w-full border rounded-md px-3 py-2"
                  required
                />
              )}
            </div>
          ))}

          <div className="flex justify-between pt-4">
            <button
              type="button"
              onClick={handleCancel}
              className="px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
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
