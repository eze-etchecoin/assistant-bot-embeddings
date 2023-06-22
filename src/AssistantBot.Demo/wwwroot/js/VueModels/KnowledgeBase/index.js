const { createApp } = Vue;
const vm = createApp({
    data() {
        return {

            IsInitialLoading: true,

            LastUploadedFileInfo: null,
            CurrentProcessFileName: "",
            CurrentProcessFileInfo: null,
            IsFileSelected: false,
            IsUploadingFile: false,
            UploadFileErrorMessage: "",

            CurrentProgress: 0,
            ProgressCheckInterval: null,

            TextToAdd: "",
            isLoading: false,
            AddParagraphSuccessful: false,
            AddParagraphErrorMessage: false,

            /*TestMessage: ""*/
        }
    },

    methods: {
        // Adds any data to knowledge base
        async addParagraph() {

            this.AddParagraphErrorMessage = false;
            this.AddParagraphSuccessful = false;
            this.isLoading = true;

            const dataObject = {
                Paragraph: this.TextToAdd
            };

            try {
                const { data } = await axios.post(
                    `${ApiUrl}/knowledgebase/addparagraphtoknowledgebase`,
                    dataObject);

                this.AddParagraphSuccessful = true;
            }
            catch (error) {
                this.errorHandler(error, "AddParagraphErrorMessage");
            }
            finally {
                this.isLoading = false;
            }

        },


        hideAddParagraphAlerts() {
            this.AddParagraphErrorMessage = "";
            this.AddParagraphSuccessful = false;
        },

        async uploadFile() {
            const fileInput = document.getElementById("fileInput");

            if (fileInput.files.length === 0) {
                this.UploadFileErrorMessage = "Please select a file to upload.";
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
                this.CurrentProcessFileInfo = null;
                this.LastUploadedFileInfo = null;

                await this.startCheckProgress();

                //setTimeout(this.getLastUploadedFileInfo, 1000)
            }
            catch (error) {
                this.errorHandler(error, "UploadFileErrorMessage");
            }
            finally {
                this.IsUploadingFile = false;
            }
        },

        async startCheckProgress() {

            //if (!this.CurrentProcessFileName)
            //    return;

            this.ProgressCheckInterval = setInterval(async () => {
                //this.CurrentProcessFileInfo = await this.getKnowledgeBaseFileInfo(this.CurrentProcessFileName);
                //this.CurrentProgress = this.CurrentProcessFileInfo.Progress;

                await this.getLastUploadedFileInfo();

                this.IsInitialLoading = false;
            }, 1000);
        },

        stopCheckProgress() {
            clearInterval(this.ProgressCheckInterval);
            this.ProgressCheckInterval = null;
        },

        async getKnowledgeBaseFileInfo(fileName) {
            try {
                const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/GetKnowledgeBaseFileInfo?fileName=${fileName}`);
                return data || null;
            }
            catch (error) {
                this.errorHandler(error);
            }
        },

        async getLastUploadedFileInfo() {
            try {
                const { data } = await axios.get(`${ApiUrl}/KnowledgeBase/GetLastUploadedFileInfo`);

                if (data) {

                    if (this.CurrentProcessFileName &&
                        this.CurrentProcessFileName != data.FileName) {
                        // Wait until the current file is processed
                        return;
                    }

                    this.LastUploadedFileInfo = data;

                    if (this.LastUploadedFileInfo.Progress < 100) {
                        // There's a file being processed
                        this.CurrentProcessFileName = this.LastUploadedFileInfo.FileName;
                        this.CurrentProgress = this.LastUploadedFileInfo.Progress;
                        //await this.startCheckProgress();
                    }
                    else {
                        // File has been processed already
                        this.stopCheckProgress();
                        this.CurrentProcessFileName = "";
                    }
                }
                else if (this.CurrentProcessFileName) {
                    return;
                }
                else {
                    this.LastUploadedFileInfo = null;
                    this.stopCheckProgress();

                }
            }
            catch (error) {
                this.errorHandler(error);
            }
        },

        errorHandler(error, errorMessageProp) {
            this[errorMessageProp] = error.response?.data || error.message;
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

        buttonDisabled() {
            return !this.TextToAdd;
        },

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

            if (this.CurrentProcessFileInfo?.ErrorMessage) {
                //clearInterval(this.ProgressCheckInterval);
                //this.ProgressCheckInterval = null;
                this.UploadFileErrorMessage = this.CurrentProcessFileInfo.ErrorMessage;
            }

            //if (this.CurrentProgress >= 100) {
            //    clearInterval(this.ProgressCheckInterval);
            //    this.ProgressCheckInterval = null;
            //    this.CurrentProcessFileName = "";

            //    this.getLastUploadedFileInfo();
            //}
        }
    },

    async mounted() {
        await this.startCheckProgress();
    }
}).mount('#vueContainer');