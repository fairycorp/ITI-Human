interface IProject {
    projectId: number;
    projectTypeId: number;
    projectTypeName: string;
    projectName: string;
    projectHeadline: string;
    projectPitch: string;
    semesterId: number;
    semesterName: string;
    storageId: number;
}

interface IDetailedDataOrder {
    info: IBasicDataOrder;
    products: IBasicDataOrderedProduct[];
}

interface IBasicDataOrder {
    orderId: number;
    userId: number;
    creationDate: Date;
    currentState: State;
}

interface IBasicDataOrderedProduct {
    orderedProductId: number;
    orderId: number;
    storageLinkedProductId: number;
    name: string;
    desc: string;
    unitPrice: number;
    quantity: number;
    currentState: State;
    payment: PaymentState;

}

enum State {
    NotStarted, Underway, Paused, Delivered, Canceled
}

enum Payment {
    Unpaid, Paid, Credited
}

interface PaymentState {
    state: Payment;
    amount: number;
}


export {
    IProject,
    IDetailedDataOrder,
    IBasicDataOrder,
    IBasicDataOrderedProduct,
    State,
    Payment,
    PaymentState,
};
