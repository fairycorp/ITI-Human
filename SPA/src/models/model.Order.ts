// --------------------------------------

// Enumerators.

enum State {
    NotStarted, Underway, Paused, Delivered, Canceled
}

enum Payment {
    Unpaid, Paid, Credited
}

// --------------------------------------

// Interfaces.

interface IBasicDataOrder {
    orderId: number;
    userId: number;
    creationDate: Date;
    displayedDate: string;
    currentState: State;
    total: number;
    fullyPaid: boolean;
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

interface IOrderCreationViewModel {
    storageId: number;
    userId: number;
    classroomId: number;
    products: IBasicDataProductToOrder[];
}

interface IOrderCurrentStateUpdateViewModel {
    userId: number;
    orderId: number;
    currentState: State;
}

interface IBasicDataProductToOrder {
    storageLinkedProductId: number;
    quantity: number;
}

interface IDetailedDataOrder {
    info: IBasicDataOrder;
    products: IBasicDataOrderedProduct[];
}
interface IUserBalance {
    userBalanceId: number;
    projectId: number;
    userId: number;
    userName: string;
    firstName: string;
    lastName: string;
    avatarUrl: string;
    balance: number;
}

interface IOrderCreditGettingViewModel {
    userId: number;
    projectId: number;
}

interface IOrderCredit {
    orderCreditId: number;
    userId: number;
    projectId: number;
    amount: number;
    creditTime: Date;
    displayedDate: string;
}

interface IUserBalanceUpdateViewModel {
    userBalanceId: number;
    amount: number;
}

interface PaymentState {
    state: Payment;
    amount: number;
}

interface IPaymentStateUpdateViewModel {
    actorId: number;
    userId: number;
    orderedProductId: number;
    paymentState: PaymentState;
}

interface ICurrentStateUpdateViewModel {
    userId: number;
    orderedProductId: number;
    currentState: State;
}

export {
    State,
    Payment,
    IBasicDataOrder,
    IBasicDataOrderedProduct,
    IBasicDataProductToOrder,
    IOrderCreationViewModel,
    IDetailedDataOrder,
    IUserBalance,
    IOrderCredit,
    IUserBalanceUpdateViewModel,
    IOrderCreditGettingViewModel,
    IOrderCurrentStateUpdateViewModel,
    IPaymentStateUpdateViewModel,
    ICurrentStateUpdateViewModel,
    PaymentState,
};
