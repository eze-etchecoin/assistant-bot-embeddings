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

            if (fileInput.files.length === 0) {
                this.ErrorMessage = "Please select a file to upload.";
                return;
            }

            const file = fileInput.files[0];
            const formData = new FormData();
            formData.append("file", file);

            try {
                const response = await axios.post(
                    `${ApiUrl}/KnowledgeBase/UploadFile`,
                    formData,
                    {
                        headers: { "Content-Type": "multipart/form-data" }
                    });

                this.FileName = response.data;
            }
            catch (error) {
                this.ErrorMessage = error.response || error.message;
            }
        }
    }
}).mount('#vueContainer');