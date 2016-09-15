package foundation.softwaredesign.orbi.model;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class InventoryItem {

    private String prefab;
    private Long amount;

    public InventoryItem() {
    }

    public InventoryItem(String prefab, Long amount) {
        this.prefab = prefab;
        this.amount = amount;
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
}
