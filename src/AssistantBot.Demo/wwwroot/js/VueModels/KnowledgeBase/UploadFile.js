const { createApp } = Vue;
const vm = createApp({
    data() {
        return {
            LastUploadedFileInfo: null,
            CurrentProcessFileName: "",
            IsUploadingFile: false,
            ErrorMessage: "",

            CurrentProgress: 0,
            ProgressCheckInterval: null,

            /*TestMessage: ""*/
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

            this.IsUploadingFile = true;

            try {
                const { data } = await axios.post(
                    `${ApiUrl}/KnowledgeBase/UploadFile`,
                    formData,
                    {
                        headers: { "Content-Type": "multipart/form-data" }
                    });

                this.CurrentProcessFileName = data;

                await this.startCheckProgress();
            }
            catch (error) {
                this.ErrorHandler(error);
            }
            finally {
                this.IsUploadingFile = false;
            }
        },

        async startCheckProgress() {

            if (!this.CurrentProcessFileName)
                return;

            this.ProgressCheckInterval = setInterval(async () => {
                try {
                    const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/CheckProgress?fileName=${this.CurrentProcessFileName}`);
                    this.CurrentProgress = data;
                }
                catch (error) {
                    this.ErrorHandler(error);
                }
            }, 1000);
        },

        async getLastUploadedFileInfo() {
            try {
                const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/GetLastUploadedFileInfo`);
                this.LastUploadedFileInfo = data;
            }
            catch (error) {
                this.ErrorHandler(error);
            }
        },

        ErrorHandler(error) {
            this.ErrorMessage = error.response?.data || error.message;
        }

        //GetTestFromApi: async function () {
        //    try {
        //        const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/Test`);
        //        this.TestMessage = data;
        //    }
        //    catch (error) {
        //        this.TestMessage = error.response || error.message;
        //    }
        //}
    },

    computed: {
        ProgressBarStyle() {
            return `width: ${this.CurrentProgress}%;`;
        },
        ShowsUploadSpinner() {
            return this.IsUploadingFile;
        },
        DisabledUploadButton() {
            return this.IsUploadingFile || this.CurrentProcessFileName && this.CurrentProgress < 100;
        }
    },

    watch: {
        CurrentProgress() {
            if (this.CurrentProgress >= 100) {
                clearInterval(this.ProgressCheckInterval);
                this.ProgressCheckInterval = null;
                this.CurrentProcessFileName = "";
            }
        }
    }
}).mount('#vueContainer');