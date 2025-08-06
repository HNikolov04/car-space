import AboutHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import useCarMeetCreate from "../../hooks/useCarMeetCreate";

export default function CreateCarMeetPage() {
  const {
    register,
    handleSubmit,
    onSubmit,
    navigateBack,
    loadingUser,
  } = useCarMeetCreate();

  if (loadingUser) return null;

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <AboutHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="bg-white border border-gray-200 shadow-xl rounded-2xl p-7 w-full max-w-lg scale-[0.95]">
          <h2 className="text-2xl font-bold text-center mb-6 text-gray-900">
            Create Car Meet
          </h2>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 text-sm">
            {[
              { name: "title", label: "Title", type: "text" },
              { name: "description", label: "Description", type: "textarea" },
              { name: "meetDate", label: "Meet Date", type: "date" },
              { name: "city", label: "City", type: "text" },
              { name: "address", label: "Address", type: "text" },
              { name: "imageFile", label: "Upload Image", type: "file" },
            ].map(({ name, label, type }) => (
              <div key={name}>
                <label className="block italic mb-1 text-gray-700">{label}</label>
                {type === "textarea" ? (
                  <textarea
                    {...register(name, { required: true })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg resize-none"
                  />
                ) : (
                  <input
                    {...register(name, { required: name !== "imageFile" })}
                    type={type}
                    accept={type === "file" ? "image/*" : undefined}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                  />
                )}
              </div>
            ))}

            <div className="flex justify-between pt-4">
              <button
                type="button"
                onClick={navigateBack}
                className="px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
              >
                Submit
              </button>
            </div>
          </form>
        </div>
      </main>

      <Footer />
    </div>
  );
}
