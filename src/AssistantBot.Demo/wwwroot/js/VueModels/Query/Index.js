
const { createApp } = Vue;

createApp({
    data() {
        return {
            User: "",
            Text: "",
            enteredText :"",
            textResponse: "",
            alertError: "",
            
            showAnswer: false,
            isLoading: false,
        };
    },
    computed: {
        isButtonDisabled() {
            return this.Text === '';
        }
    },
    methods: {
        async consult() {
            this.enteredText = this.Text;
            this.alertError = "";
            this.showAnswer = false;
            this.isLoading = true;

             // PETICIÓN A LA API
            try {
            const dataObject = {
                Question: this.Text,
                User: this.User
            };

            const { data } = await axios.post(
                `${ApiUrl}/AssistantBot/AskToKnowledgeBase`,
                dataObject);

            this.isLoading = false;
            this.textResponse = data;
            this.showAnswer = true;
            this.Text = "";
            
            }
            catch (error) {
                this.errorHandler(error, "alertError");
                this.isLoading = false;
            } 
        },

        errorHandler(error, errorMessageProp) {
            this[errorMessageProp] = error.response?.data || error.message;
        },

        closeAlert() {
            this.showAnswer = false;   
        },

        closeAlertError() {
            this.alertError = "";
        }
    }
}).mount("#vueContainer");

