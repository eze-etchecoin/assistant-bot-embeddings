const { createApp } = Vue;
const vm = createApp({
    data() {
        return {
            LastUploadedFileInfo: null,
            CurrentProcessFileName: "",
            IsFileSelected: false,
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
                this.errorHandler(error);
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
                    this.errorHandler(error);
                }
            }, 1000);
        },

        async getLastUploadedFileInfo() {
            try {
                const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/GetLastUploadedFileInfo`);
                if (data) {
                    this.LastUploadedFileInfo = data;
                }
                else {
                    this.LastUploadedFileInfo = null;
                }
            }
            catch (error) {
                this.errorHandler(error);
            }
        },

        errorHandler(error) {
            this.ErrorMessage = error.response?.data || error.message;
        },

        checkFileSelected(event) {
            const input = event.target;
            if ('files' in input && input.files.length > 0) {
                this.IsFileSelected = true;
            }
            else {
               this.IsFileSelected = false;
            }
        },

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
            return !this.IsFileSelected ||
                this.IsUploadingFile ||
                this.CurrentProcessFileName !== "" && this.CurrentProgress < 100;
        },
        FormattedLastUploadedFileDateTime() {
            if (!this.LastUploadedFileInfo)
                return "";

            return moment(this.LastUploadedFileInfo.UploadedDateTime).format("DD/MM/YYYY HH:mm:ss");
        }
    },

    watch: {
        CurrentProgress() {
            if (this.CurrentProgress >= 100) {
                clearInterval(this.ProgressCheckInterval);
                this.ProgressCheckInterval = null;
                this.CurrentProcessFileName = "";

                this.getLastUploadedFileInfo();
            }
        }
    },

    async mounted() {
        await this.getLastUploadedFileInfo();
    }
}).mount('#vueContainer');