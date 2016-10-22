package foundation.softwaredesign.orbi.model;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class InventoryItem {

    private String prefab;
    private Long amount;
    private Boolean supportsUserText = false;
    private Long categoryId;


    public InventoryItem(String prefab, Long amount, Boolean supportsUserText, Long categoryId) {
        this.prefab = prefab;
        this.amount = amount;
        this.supportsUserText = supportsUserText;
        this.categoryId = categoryId;
    }

    public InventoryItem() {

    }

    public String getPrefab() {
        return prefab;
    }

    public void setPrefab(String prefab) {
        this.prefab = prefab;
    }

    public Long getAmount() {
        return amount;
    }

    public void setAmount(Long amount) {
        this.amount = amount;
    }

    public Boolean getSupportsUserText() {
        return supportsUserText;
    }

    public void setSupportsUserText(Boolean supportsUserText) {
        this.supportsUserText = supportsUserText;
    }

    public Long getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(Long categoryId) {
        this.categoryId = categoryId;
    }
}
