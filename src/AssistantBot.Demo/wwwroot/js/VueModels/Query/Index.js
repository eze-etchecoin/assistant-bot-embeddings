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
            alertError: false,
            ErrorMessage: "",
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
            this.alertError = false;
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
                this.ErrorMessage = "Failed to get response from API";
                //this.ErrorMessage = error.response?.data || error.message;
                this.alertError = true;
                this.isLoading = false;
            }
            
        },

        closeAlert() {
            this.mostrarRespuesta = false;   
        },

        closeAlertError() {
            this.alertError = false;
        }
    }
}).mount("#vueContainer");

