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
            textoIngresado: "",
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
        consultar() {
            this.textoIngresado = this.Texto;
            this.Texto = "";
            this.mostrarRespuesta = false;
            this.isLoading = true;
            setTimeout(this.showAlert, 3000);
        },

            /* // PETICIÓN A LA API

            const dataObject = {
                Question: this.Texto
            };

            const { data } = axios.post(
                `${ApiUrl}/AssistantBot/AskToKnowledgeBase`,
                dataObject);

            this.textoIngresado = data;
            this.mostrarRespuesta = true;

            */
        showAlert() {
            this.isLoading = false;
            this.mostrarRespuesta = true;
        },

        closeAlert() {
            this.mostrarRespuesta = false;   
        }  
    }
}).mount("#vueContainer");

