const { createApp } = Vue;
const vm = createApp({
    data() {
        return {
            FileName: "",
            ErrorMessage: "",

            TestMessage: ""
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
                const { data } = await axios.post(
                    `${ApiUrl}/KnowledgeBase/UploadFile`,
                    formData,
                    {
                        headers: { "Content-Type": "multipart/form-data" }
                    });

                this.FileName = data;
            }
            catch (error) {
                this.ErrorMessage = error.response || error.message;
            }
        },

        GetTestFromApi: async function () {
            try {
                const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/Test`);
                this.TestMessage = data;
            }
            catch (error) {
                this.TestMessage = error.response || error.message;
            }
        }
    }
}).mount('#vueContainer');