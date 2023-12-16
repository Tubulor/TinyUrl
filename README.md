# TinyURL
## Shorten URL service with LRU Cache approach 

---

### LRU For Request Collapsing

--- 
#### What is a Request Collapsing?

> Request collapsing is a technique where similar or identical requests are aggregated or collapsed into a single request to reduce the number of requests processed by the server.

#### why LRU Cache is good for Request Collapsing?
> LRU is designed to capture temporal locality, meaning that recently accessed items are likely to be accessed again. 
> In the context of request collapsing, this aligns with the assumption that similar requests made recently are likely to be repeated.

### LRU Advantages
> - Efficient Use of Cache -LRU only keeping the most recently used items in cache
> - Wide usage which make it a safe choice

### LRU Disadvantages

> - the cache size is typically limited, and choosing an inappropriate size may lead to not optimal performance. 
> If the cache is too small, frequently used mappings may be evicted more often, resulting in increased database queries.
> - When the cache reaches its maximum capacity and needs to evict an item, the LRU algorithm requires searching for the least recently used item, which can be computationally expensive. 
> This search operation becomes more time-consuming as the cache size grows.
> - High memory usage: The LRU cache requires additional memory to keep track of the most recently used items. 
> - This overhead can be significant, especially when dealing with a large number of cache entries or large-sized objects.