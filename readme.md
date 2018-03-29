# Moonpig Engineering recruitment test

We've not set a time limit, do whatever you feel is reasonable however consider
 this to be production quality code. 

Make commits where you feel appropriate to make it easier for the assessor to 
understand your thought process. 

When complete please upload your solution and answers in a .zip to the google
drive link provided to you by the recruiter.

---

Parts 1 & 2 have already been completed albeit lacking in quality, please 
check the implementation of these and look at refactoring it into something 
that you consider to be clean and well tested.

Then extend your solution to capture the requirements listed in part 3.

Do not change the public interface `IPostOffice`. The provided DbContext
is a stubbed class which provides test data. Please feel free to use this
in your implementation and tests but do keep in mind that it would be 
switched for something like an EntityFramework DBContext backed by a 
real database in production.

Once you have completed the exercise please answer the questions listed below. 
We are not looking for essay length answers. You can add the answers in this 
document.

## Questions

Q1. What 'code smells' did you find in the existing implementation of part 1 & 2?

Q2. What further steps would you take to improve the solution given more time?

Q3. What's a technology that you're excited about and where do you see this 
    being applicable?

Q4. What process would you take to identify a performance issue in a production
    environment? 

## Programming Exercise - Moonpig Post Office

You have been tasked with creating a service that calculates the estimated 
despatch dates of customers' orders. 

An order contains a collection of products that a customer has added to their 
shopping basket. 

Each of these products is supplied to Moonpig on demand through a number of 
3rd party suppliers.

As soon as an order is received by a supplier, the supplier will start 
processing the order. The supplier has an agreed lead time in which to 
process the order before delivering it to the Moonpig Post Office.

Once the Moonpig Post Office has received all products in an order it is 
despatched to the customer.  

Assumptions:

1. Suppliers start processing an order on the same day that the order is 
received. For example, a supplier with a lead time of one day, receiving
 an order today will send it to Moonpig tomorrow.


2. Once all products for an order have arrived at Moonpig from the suppliers, 
they will be despatched to the customer on the same day.

### Part 1

Write an implementation of `IPostOffice` that calculates the despatch date 
of an order. 

Some of the code in the Post Office system has already been written; your 
implementation should interact with this code.


### Part 2

Moonpig Post Office staff are getting complaints from customers expecting 
packages to be delivered on the weekend. You find out that the Moonpig post
office is shut over the weekend. Packages received from a supplier on a weekend 
will be despatched the following Monday.

Modify the existing code to ensure that any orders received from a supplier
on the weekend are despatched on the following Monday.

### Part 3

The Moonpig post office is still getting complaints... It turns out suppliers 
don't work during the weekend as well.

Modify the existing code to ensure that any orders that would have been processed
during the weekend resume processing on Monday.
