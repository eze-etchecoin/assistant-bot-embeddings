// Esto era en Vue 2
//const vm = new Vue({
//    el: '#vueContainer',
//    data: {
//        Texto: ''
//    }
//})

const { createApp } = Vue;

createApp({
    data() {
        return {
            Texto: "",
            textoIngresado :"",
            textoRespuesta: "",
            alertError: "",
            
            mostrarRespuesta: false,
            isLoading: false,
        };
    },
    computed: {
        isButtonDisabled() {
            return this.Texto === '';
        }
    },
    methods: {
        async consultar() {
            this.textoIngresado = this.Texto;
            this.alertError = "";
            this.mostrarRespuesta = false;
            this.isLoading = true;

             // PETICIÓN A LA API
            try {
            const dataObject = {
                Question: this.Texto
            };

            const { data } = await axios.post(
                `${ApiUrl}/AssistantBot/AskToKnowledgeBase`,
                dataObject);

            this.isLoading = false;
            this.textoRespuesta = data;
            this.mostrarRespuesta = true;
            this.Texto = "";
            
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
            this.mostrarRespuesta = false;   
        },

        closeAlertError() {
            this.alertError = "";
        }
    }
}).mount("#vueContainer");

