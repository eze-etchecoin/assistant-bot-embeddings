const { createApp } = Vue;
const vm = createApp({
    data() {
        return {
            FileName: "",
            ErrorMessage: ""
        }
    },

    methods: {
        async uploadFile() {
            const fileInput = document.getElementById("fileInput");
            const file = fileInput.files[0];
            const formData = new FormData();
            formData.append("file", file);

            try {
                const fileName = await axios.post(
                    `${ApiUrl}/KnowledgeBase/UploadFile`,
                    formData,
                    {
                        headers: { "Content-Type": "multipart/form-data" }
                    });

                this.FileName = fileName;
            }
            catch (error) {
                this.ErrorMessage = error.response?.data || error.message;
            }
        }
    }
}).mount('#vueContainer');