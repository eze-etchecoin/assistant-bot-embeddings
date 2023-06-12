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
            this.mostrarRespuesta = false;
            this.isLoading = true;

             // PETICIÓN A LA API

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
            
        },

        //showAlert() {
        //    this.isLoading = false;
        //    this.mostrarRespuesta = true;
        //},

        closeAlert() {
            this.mostrarRespuesta = false;   
        }  
    }
}).mount("#vueContainer");

