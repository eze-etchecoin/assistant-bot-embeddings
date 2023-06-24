
const { createApp } = Vue;

createApp({
    data() {
        return {
            rowsUploadedFiles:
                [
                    {
                        FileName: "document1.pdf",
                        UploadedDateTime: "2023-06-23T14:32:01.7334218+00:00",
                        TotalParagraphs: 11
                    },
                    {
                        FileName: "document2.pdf",
                        UploadedDateTime: "2023-06-23T14:35:00.0000000+00:00",
                        TotalParagraphs: 40
                    },
                    {
                        FileName: "document3.docx",
                        UploadedDateTime: "2023-06-23T14:37:00.0000000+00:00",
                        TotalParagraphs: 5
                    }
                ]
        };
    },
    
    
}).mount("#vueContainer");

